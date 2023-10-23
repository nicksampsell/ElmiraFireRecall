using ElmiraFireRecall.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using ElmiraFireRecall.Models;

namespace ElmiraFireRecall.Controllers
{
    public class SendEmailController : Controller
    {
        private readonly FireDBContext _context;
        private readonly IConfiguration _config;

        public SendEmailController(FireDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [Route("~/SendEmail/{messageType}")]
        [ActionName("SendEmail")]
        public async Task<IActionResult> SendTestMessage([FromForm] SubmissionDTO submission, string messageType = "test")
        {
            string[] allowedMessageTypes = { "test", "fireRecall", "emo", "misc", "individual" };
            string messageWrapper = "[MESSAGE]";
            string historyMessageType = "misc";

            if (!allowedMessageTypes.Contains(messageType))
            {
                return BadRequest("Invalid Message Type");
            }

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_config.GetValue<string>("Mail:Server"), _config.GetValue<int>("Mail:Port"), SecureSocketOptions.Auto);

                string subject = String.Empty;
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Fire Recall System", "FirePager@cityofelmira.net"));



                if (!String.IsNullOrEmpty(_config.GetValue<string?>("Mail:Username")) &&
                    !String.IsNullOrEmpty(_config.GetValue<string?>("Mail:Password")))
                {
                    client.Authenticate(_config.GetValue<string>("Mail:Username"), _config.GetValue<string>("Mail:Password"));
                }

                var recipients = _context.Recipients.Include(x => x.PhoneProvider).Include(x => x.FireGroups).AsQueryable();
                

                Dictionary<string, int> groups = _context.Groups.ToDictionary(x => x.Title.ToLower(), x => x.Id);

                if (messageType == "emo")
                {
                    //EMO tab only sends to EMO
                    recipients = recipients.Where(x => x.FireGroups.Any(y => y.Id == groups["emo"]));
                    subject = String.IsNullOrEmpty(submission.Subject) ? "Message from Fire Recall System" : submission.Subject;
                    historyMessageType = "emo";
                }
                else if (messageType == "misc") //fixme
                {
                    //misc allows individuals to select the group they wish to send the message to
                    recipients = recipients.Where(x => x.FireGroups.Any(y => submission.RecipientGroup.Contains(y.Id)));
                    subject = String.IsNullOrEmpty(submission.Subject) ? "Message from Fire Recall System" : submission.Subject;
                    historyMessageType = "misc";
                }
                else if (messageType == "individual") //fixme
                {
                    //recipients are contained within the array of ids submitted by the form
                    recipients = recipients.Where(x => submission.RecipientPerson.Contains(x.Id));
                    subject = String.IsNullOrEmpty(submission.Subject) ? "Message from Fire Recall System" : submission.Subject;
                    historyMessageType = "misc";
                }
                else
                {
                    //test and fire recall emergency only send to recall group
                    recipients = recipients.Where(x => x.FireGroups.Any(y => y.Id == groups["recall"]));
                    subject = (messageType == "fireRecall") ? "Fire Recall Emergency" : "Recall System Test";

                    if (messageType == "test")
                    {
                        messageWrapper = "*** THIS IS ONLY A TEST ***\n\n [MESSAGE] \n\n *** THIS IS ONLY A TEST ***";
                        historyMessageType = "test";
                    }

                    if(messageType == "fireRecall")
                    {
                        historyMessageType = "emergency";
                    }
                }

                List<MailboxAddress> AllRecipients = new List<MailboxAddress>();
                foreach (var recipient in recipients.ToList())
                {

                    AllRecipients.Add(new MailboxAddress(
                            recipient.FullName,
                            $"{recipient.PhoneNumber}@{recipient.PhoneProvider.EmailSuffix.Replace("@", "").Trim()}"));
                }



                message.To.Add(new MailboxAddress("Nick Sampsell", "nsampsell@chemungcountyny.gov"));
                message.Bcc.AddRange(AllRecipients);



                var bodyBuilder = new BodyBuilder();

                messageWrapper = messageWrapper.Replace("[MESSAGE]", $"Submitted on {DateTime.Now.ToString("D")} at {DateTime.Now.ToString("t")} by {User.FindFirst("FullName").Value}");

                if (!String.IsNullOrEmpty(submission.Message))
                {
                    messageWrapper = messageWrapper + ": \n\n " + submission.Message;
                }

                bodyBuilder.TextBody = messageWrapper;

                message.Body = bodyBuilder.ToMessageBody();

                await client.SendAsync(message);
                client.Disconnect(true);
            };


            _context.MessageHistory.Add(new MessageHistory()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Subject = String.IsNullOrEmpty(submission.Subject) ? "" : submission.Subject,
                Message = messageWrapper,
                Recipients = (submission.RecipientPerson == null || submission.RecipientPerson.ToList().Count <= 0) ? new List<FireRecipient>() : _context.Recipients.Where(x => submission.RecipientPerson.Contains(x.Id)).ToList(),
                Groups = (submission.RecipientGroup == null || submission.RecipientGroup.ToList().Count <= 0) ? new List<FireGroup>() : _context.Groups.Where(x => submission.RecipientGroup.Contains(x.Id)).ToList(),
                MessageType = _context.MessageTypes.Where(x => x.Name.ToLower() == historyMessageType).SingleOrDefault(),
                UserId = int.Parse(User.FindFirst("UserId").Value)

            });
            await _context.SaveChangesAsync();

            TempData["success"] = "The message was sent successfully.";
            TempData["TabName"] = messageType;
            return Redirect("/");
        }


    }
    public class SubmissionDTO
    {
        public int[]? RecipientGroup { get; set; }
        public int[]? RecipientPerson { get; set; }
        public string? Subject { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;

    }
}
