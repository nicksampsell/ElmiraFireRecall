using ElmiraFireRecall.Models;

namespace ElmiraFireRecall.Services
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
    }
}
