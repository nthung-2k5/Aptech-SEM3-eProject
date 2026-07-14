using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Hydro.Utils;

namespace GiveAID.Pages.Donate;

public class DonateForm(
    INgoService ngoService,
    IProgrammeService programmeService,
    IDonationCauseService donationCauseService,
    IPaymentService paymentService,
    IDonationService donationService,
    IMemberService memberService,
    IValidator<DonateForm> validator
) : HydroComponent
{
    public Guid? NgoId { get; set; }
    public Guid? CauseId { get; set; }
    public Guid? ProgrammeId { get; set; }
    public Guid UserId { get; set; }
    
    public string UserFullName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserPhone { get; set; } = string.Empty;

    public decimal Amount { get; set; } = 10;
    public string CardNumber { get; set; } = string.Empty;
    public string CardHolderName { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;

    public string TargetDescription { get; set; } = "General Donation";

    // Populated when donating to an NGO so the user can pick a cause inline
    public DonationCauseDto[] Causes { get; set; } = [];

    public override async Task MountAsync()
    {
        await LoadCausesAsync();
        await LoadTargetDescriptionAsync();
        
        if (UserId != Guid.Empty)
        {
            var user = await memberService.GetMemberByIdAsync(UserId);
            if (user != null)
            {
                UserFullName = user.FullName;
                UserEmail = user.Email;
                UserPhone = user.PhoneNumber;
            }
        }
    }

    public override async Task BindAsync(PropertyPath property, object value)
    {
        if (property.Name == nameof(CauseId))
        {
            await LoadTargetDescriptionAsync();
        }
    }

    public async Task Submit()
    {
        if (!this.Validate(validator)) { return; }

        // Process payment
        var paymentRequest = new PaymentRequestDto(Amount, CardNumber, CardHolderName, ExpiryDate, CVV, "CreditCard");
        var transaction = await paymentService.ProcessPaymentAsync(paymentRequest);

        DonationTarget target;

        if (ProgrammeId.HasValue && ProgrammeId.Value != Guid.Empty)
        {
            target = new DonateForProgrammeTarget(ProgrammeId.Value);
        }
        else if (NgoId.HasValue && NgoId.Value != Guid.Empty)
        {
            target = new DonateForNgoTarget(NgoId.Value, CauseId ?? Guid.Empty);
        }
        else
        {
            ModelState.AddModelError("", "No donation target selected.");
            return;
        }

        var donationSave = new DonationSaveDto(UserId, target, Amount, transaction.TransactionId);
        
        await donationService.CreateDonationAsync(donationSave);
        Location($"/Donate/Success?transactionId={transaction.TransactionId}");
    }

    private async Task LoadCausesAsync()
    {
        if (NgoId.HasValue && NgoId.Value != Guid.Empty)
        {
            Causes = await donationCauseService.GetAllDonationCausesAsync();
        }
    }

    private async Task LoadTargetDescriptionAsync()
    {
        if (ProgrammeId.HasValue && ProgrammeId.Value != Guid.Empty)
        {
            var prog = await programmeService.GetProgrammeByIdAsync(ProgrammeId.Value);
            TargetDescription = prog != null ? $"Programme: {prog.Name}" : "Unknown Programme";
        }
        else if (NgoId.HasValue && NgoId.Value != Guid.Empty)
        {
            var ngo = await ngoService.GetNgoByIdAsync(NgoId.Value);
            string ngoName = ngo != null ? ngo.Name : "Unknown NGO";

            if (CauseId.HasValue && CauseId.Value != Guid.Empty)
            {
                var cause = await donationCauseService.GetDonationCauseByIdAsync(CauseId.Value);
                string causeName = cause != null ? cause.Name : "Unknown Cause";
                TargetDescription = $"{ngoName} — {causeName}";
            }
            else
            {
                TargetDescription = $"{ngoName} — General";
            }
        }
    }

    public class Validator : AbstractValidator<DonateForm>
    {
        public Validator()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(1).WithMessage("Amount must be at least 1");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required")
                .Must(v => !string.IsNullOrWhiteSpace(v) && v.Replace(" ", "").Length == 16)
                .WithMessage("Enter a valid 16-digit card number");

            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("Cardholder name is required");

            RuleFor(x => x.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date is required")
                .Matches(@"^\d{2}/\d{2}$").WithMessage("Use MM/YY format")
                .Must(expiryDate =>
                {
                    if (string.IsNullOrEmpty(expiryDate)) return false;

                    // Split the MM/YY string
                    string[] parts = expiryDate.Split('/');
                    if (parts.Length != 2) return false;

                    if (int.TryParse(parts[0], out int month) && int.TryParse(parts[1], out int year))
                    {
                        // Convert 2-digit year to 4-digit year (e.g., 26 -> 2026)
                        int fullYear = 2000 + year;

                        try
                        {
                            // Get the last millisecond of the expiry month
                            int daysInMonth = DateTime.DaysInMonth(fullYear, month);
                            var expiryDateTime = new DateTime(fullYear, month, daysInMonth, 23, 59, 59);

                            return expiryDateTime > DateTime.Now;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            // Handles invalid months like 13
                            return false;
                        }
                    }
                    
                    return false;
                }).WithMessage("Expiry date must be in the future");

            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("CVV is required")
                .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3–4 digits");
        }
    }
}
