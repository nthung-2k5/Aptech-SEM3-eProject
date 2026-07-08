using GiveAID.Services.Abstractions;



namespace GiveAID.Services;

public class EmailService : IEmailService
{
   public async Task<bool> SendEmailAsync(string receiverEmail, string subject, string body, CancellationToken ct = default)
    {
        var smtpSection = configuration.GetSection("Smtp");

        var host = smtpSection["Host"];
        var port = smtpSection.GetValue("Port", 587);
        var username = smtpSection["Username"];
        var password = smtpSection["Password"];
        var senderEmail = smtpSection["SenderEmail"] ?? username;
        var senderName = smtpSection["SenderName"] ?? "Give-AID";
        var enableSsl = smtpSection.GetValue("EnableSsl", true);

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(senderEmail))
        {
            throw new InvalidOperationException("SMTP settings are missing. Configure the Smtp section in appsettings.json.");
        }

        using var message = new MailMessage
        {
            From = new MailAddress(senderEmail, senderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(receiverEmail);

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = enableSsl,
            Credentials = new NetworkCredential(username, password)
        };

        try
        {
            using (ct.Register(client.Dispose))
            {
                await client.SendMailAsync(message);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
