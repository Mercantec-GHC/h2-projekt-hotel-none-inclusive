using API.Configuration;
using API.Models;
using MailKit.Net.Smtp;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;

namespace API.Services
{
    // Implementation of IMailService that uses MailKit for sending emails.
    public class MailService : IMailService
    {
        // Mail settings to configure SMTP client, such as host, port, and credentials.
        MailSettings Mail_Settings = null;

        // Constructor to inject MailSettings via IOptions, enabling the configuration of mail settings from a configuration file.
        public MailService(IOptions<MailSettings> options)
        {
            Mail_Settings = options.Value;  // Assign the injected mail settings to the class-level variable.
        }

        // Method to send an email using MailData input.
        public bool SendEmail(MailData mailData)
        {
            try
            {
                // Create a new MimeMessage object to build the email.
                MimeMessage email_Message = new MimeMessage();

                // Set the sender's email address using the settings.
                MailboxAddress email_From = new MailboxAddress(Mail_Settings.Name, Mail_Settings.EmailId);
                email_Message.From.Add(email_From);

                // Set the recipient's email address from the MailData input.
                MailboxAddress email_To = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                email_Message.To.Add(email_To);

                // Set the subject of the email.
                email_Message.Subject = mailData.EmailSubject;

                // Create the email body and add it to the message.
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.EmailBody;
                email_Message.Body = emailBodyBuilder.ToMessageBody();

                // Initialize the SMTP client to send the email.
                SmtpClient MailClient = new SmtpClient();

                // Connect to the SMTP server using the host, port, and SSL settings from MailSettings.
                MailClient.Connect(Mail_Settings.Host, Mail_Settings.Port, Mail_Settings.UseSSL);

                // Authenticate with the SMTP server using the email credentials from MailSettings.
                MailClient.Authenticate(Mail_Settings.EmailId, Mail_Settings.Password);

                // Send the email message.
                MailClient.Send(email_Message);

                // Disconnect and dispose of the SMTP client after the email is sent.
                MailClient.Disconnect(true);
                MailClient.Dispose();

                // If everything succeeds, return true to indicate that the email was sent.
                return true;
            }
            catch (Exception ex)
            {
                // If an error occurs during the email sending process, return false.
                return false;
            }
        }
    }
}