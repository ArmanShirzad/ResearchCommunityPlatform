using Microsoft.AspNetCore.Identity.UI.Services;

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mailSettings = _configuration.GetSection("EmailSettings");
        using (var client = new SmtpClient("smtp.yourserver.com"))
        {
            client.Timeout = 30000;
            client.Host = mailSettings["Host"];
            client.Port = int.Parse(mailSettings["Port"]);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(mailSettings["Username"], mailSettings["Password"]);

                //client.UseDefaultCredentials = false;
            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailSettings["SenderEmail"], mailSettings["SenderDisplayName"]),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
          mailMessage.To.Add(new MailAddress(email));
            try
            {
                await  client.SendMailAsync(mailMessage);

            }
            catch (Exception)
            {

                throw;
            }

        };

    }
}
