using System.Threading.Tasks;

namespace Nestor.Tools.Infrastructure.Abstraction
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}