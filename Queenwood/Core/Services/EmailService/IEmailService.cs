using Queenwood.Models.Shared;
using System.Threading.Tasks;

namespace Queenwood.Core.Services.EmailService
{
    public interface IEmailService
    {
        Task<Result> SendEnquiry(string subject, string body);
        void SendErrorAlert(string details);
    }
}
