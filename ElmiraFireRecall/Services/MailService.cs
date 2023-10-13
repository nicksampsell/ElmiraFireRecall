using ElmiraFireRecall.Models;
using ElmiraFireRecall.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Pkcs;

namespace ElmiraFireRecall.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
        {
            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
                mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                foreach (string mailAddress in mailData.To)
                {
                    mail.To.Add(MailboxAddress.Parse(mailAddress));
                }

                if (!string.IsNullOrEmpty(mailData.ReplyTo))
                {
                    mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));
                }

                if (mailData.Bcc != null)
                {
                    foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    {
                        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                    }
                }

                var body = new BodyBuilder();
                mail.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;


                if (mailData.EmailAttachments != null)
                {
                    foreach (EmailAttachment attach in mailData.EmailAttachments)
                    {

                        var itm = body.Attachments.Add(attach.Path);
                        itm.ContentDisposition.FileName = attach.FileTitle;
                    }
                }

                mail.Body = body.ToMessageBody();
                using var smtp = new SmtpClient();

                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;


                if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                    await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
                }
                else if (_settings.UseStartTls)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                    await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
                }
                else
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, false, ct);
                }

                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);

                return true;

            }
            catch (Exception ex)
            {
                //Log.Error("{Message} {InnerMessage} {Stack}", new { Message = ex.Message, InnerMessage = ex.InnerException, Stack = ex.StackTrace});
                return false;
            }
        }
    }
}
