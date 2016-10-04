using System.Threading.Tasks;

namespace MPWebAPI.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}