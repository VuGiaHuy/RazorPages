using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GiaHuy.Service;

public class SendMailService : IEmailSender
{
    private readonly MailSetting mailSetting;
    private readonly ILogger<SendMailService> logger;
    public SendMailService(IOptions<MailSetting> _mailSetting,ILogger<SendMailService> _logger)
    {
        mailSetting = _mailSetting.Value;
        logger = _logger;
        logger.LogInformation("Create SendMailService");
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.Sender = new MailboxAddress(mailSetting.DisplayName, mailSetting.Mail);
        message.From.Add(new MailboxAddress(mailSetting.DisplayName, mailSetting.Mail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = htmlMessage;
        message.Body = builder.ToMessageBody();
        
        //MailKit
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        try{
            smtp.Connect(mailSetting.Host,mailSetting.Port);
            smtp.Authenticate(mailSetting.Mail, mailSetting.Password);
            await smtp.SendAsync(message);
        }
        catch(Exception e)
        {
            Directory.CreateDirectory("mailSave");
            var emailSaveFile = String.Format(@"mailSave/mailsave{0}.eml",Guid.NewGuid());
            await message.WriteToAsync(emailSaveFile);
            logger.LogInformation("Fail to send Email!!!");
            logger.LogError(e.Message);
        }
    }
}