using System.Threading.Tasks;

namespace Nestor.Tools.Infrastructure.Abstraction
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = false);
    }
}