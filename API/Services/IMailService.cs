using API.Models;

namespace API.Services
{
    // Interface defining the contract for mail services.
    public interface IMailService
    {
        // Method for sending an email, accepts MailData as input and returns a boolean indicating success or failure.
        bool SendMail(MailData Mail_Data);
    }
}