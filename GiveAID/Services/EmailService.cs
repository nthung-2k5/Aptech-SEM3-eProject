using GiveAID.Services.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace GiveAID.Services;

public class EmailService : IEmailService
{
    public EmailService(IConfiguration configuration)
    {
        var smtpSection = configuration.GetSection("Smtp");

        host = smtpSection["Host"]!;
        port = smtpSection.GetValue("Port", 587);
        username = smtpSection["Username"]!;
        password = smtpSection["Password"]!;
        senderEmail = smtpSection["SenderEmail"] ?? username;
        senderName = smtpSection["SenderName"] ?? "Give-AID";
        enableSsl = smtpSection.GetValue("EnableSsl", true);

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(senderEmail))
        {
            throw new InvalidOperationException("SMTP settings are missing. Configure the Smtp section in appsettings.json.");
        }
    }
    
    private readonly string host;
    private readonly int port;
    private readonly string username;
    private readonly string password;
    private readonly string senderEmail;
    private readonly string senderName;
    private readonly bool enableSsl;

    public async Task<bool> SendEmailAsync(string receiverEmail, string subject, string body, CancellationToken ct = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmail));
        message.To.Add(MailboxAddress.Parse(receiverEmail));
        message.Subject = subject;

        message.Body = new TextPart(TextFormat.Html)
        {
            Text = body
        };

        using var client = new SmtpClient();

        var secureSocketOptions = enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
        await client.ConnectAsync(host, port, secureSocketOptions, ct);
        await client.AuthenticateAsync(username, password, ct);
        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);

        return true;
    }
}