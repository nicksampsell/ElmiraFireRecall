namespace ElmiraFireRecall.Models
{
    public class MailData
    {
        public List<string> To { get; }
        public List<string> Bcc { get; }
        public List<string> Cc { get; }

        public string? From { get; }
        public string? DisplayName { get; }
        public string? ReplyTo { get; }
        public string? ReplyToName { get; }

        public string? Subject { get; }
        public string? Body { get; }

        public List<EmailAttachment>? EmailAttachments { get; set; }

        public MailData(List<string> to, string subject, string? body = null, string? from = null, string? displayName = null, string? replyTo = null, string? replyToName = null, List<string>? bcc = null, List<string>? cc = null, List<EmailAttachment>? attachments = null)
        {
            To = to;
            Bcc = bcc ?? new List<string>();
            Cc = cc ?? new List<string>();

            From = from;
            DisplayName = displayName;
            ReplyTo = replyTo;
            ReplyToName = replyToName;

            Subject = subject;
            Body = body;

            EmailAttachments = attachments;
        }
    }

    public struct EmailAttachment
    {
        public string ContentType { get; set; }
        public string Path { get; set; }

        public string FileTitle { get; set; }
    }
}
