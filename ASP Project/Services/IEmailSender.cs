using System.Threading.Tasks;

namespace ASP_Project.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}