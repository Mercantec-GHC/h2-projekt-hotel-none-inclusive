using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class MailController : ControllerBase
{
    // Reference to the mail service, used to send emails.
    IMailService Mail_Service = null;

    // Constructor to inject the IMailService dependency, enabling mail functionality.
    public MailController(IMailService _MailService)
    {
        Mail_Service = _MailService;  // Assign the injected mail service to the class-level variable.
    }

    // HTTP POST method to send an email.
    [HttpPost]
    public bool SendMail(MailData mailData)
    {
        // Calls the SendMail method from the injected mail service to send the provided mail data.
        return Mail_Service.SendEmail(mailData);
    }
}