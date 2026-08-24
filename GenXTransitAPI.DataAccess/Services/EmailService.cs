using GenXTransitAPI.DataAccess.Interface.IServices;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace GenXTransitAPI.DataAccess.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(
            string to,
            string subject,
            string body)
        {
            var smtp = _config.GetSection("Smtp");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                smtp["FromName"],
                smtp["FromEmail"]));

            message.To.Add(
                MailboxAddress.Parse(to));

            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                smtp["Host"],
                int.Parse(smtp["Port"]!),
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                smtp["User"],
                smtp["Pass"]);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }

        public async Task SendUserCreatedEmail(
            string toEmail,
            string loginId,
            string password)
        {
            var smtp = _config.GetSection("Smtp");

            var host = smtp["Host"]?.Trim();
            var port = int.Parse(smtp["Port"]!);
            var user = smtp["User"]?.Trim();
            var pass = smtp["Pass"]?.Trim();
            var fromEmail = smtp["FromEmail"]?.Trim();
            var fromName = smtp["FromName"]?.Trim();

            var loginUrl =
                _config["Application:LoginUrl"];

            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    fromName,
                    fromEmail));

            message.To.Add(
                MailboxAddress.Parse(toEmail));

            message.Subject =
                "Your Account Has Been Created - GenXAI";

            message.Body = new BodyBuilder
            {
                HtmlBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; font-size:14px;'>

                        <h2>Welcome to GenXAI Platform</h2>

                        <p>Dear User,</p>

                        <p>
                            Your account has been created successfully.
                        </p>

                        <table cellpadding='6' cellspacing='0'>
                            <tr>
                                <td><b>Login ID</b></td>
                                <td>{loginId}</td>
                            </tr>

                            <tr>
                                <td><b>Temporary Password</b></td>
                                <td>{password}</td>
                            </tr>
                        </table>

                        <br/>

                        <p>
                            <a href='{loginUrl}'
                               style='background:#007bff;
                                      color:white;
                                      padding:10px 20px;
                                      text-decoration:none;
                                      border-radius:4px;'>
                                Login to GenXAI
                            </a>
                        </p>

                        <p>
                            Or copy and paste this URL into your browser:
                        </p>

                        <p>
                            <a href='{loginUrl}'>{loginUrl}</a>
                        </p>

                        <p style='color:red'>
                            <b>Important:</b>
                            Please change your password after your first login.
                        </p>

                        <br/>

                        <p>
                            Regards,<br/>
                            <b>GenXAI Support Team</b>
                        </p>

                    </body>
                    </html>"
            }.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                user,
                pass);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
    
   