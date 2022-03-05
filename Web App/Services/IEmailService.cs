using System.Threading.Tasks;

namespace Web_App.Services
{
    public interface IEmailService
    {
        Task SendAsync(string from, string to, string subject, string body);
    }
}