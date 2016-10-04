using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;

namespace MPWebAPI.Services
{
    
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridAPIKey { get; set; }
    }
    
    public class AuthMessageSender : IEmailSender
    {
        
        public readonly ILogger _logger;

        public AuthMessageSender(IOptions<AuthMessageSenderOptions> options, ILoggerFactory loggerFactory)
        {
            Options = options.Value;
            _logger = loggerFactory.CreateLogger("AuthMessageSender");
        }

        public AuthMessageSenderOptions Options { get; }
        
        
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var sgMessage = new SendGridMessage();
            sgMessage.AddTo(email);
            sgMessage.From = new System.Net.Mail.MailAddress("mailer@merlinmail.lemonadelabs.io", "donotreply");
            sgMessage.Subject = subject;
            sgMessage.Text = message;
            sgMessage.Html = message;
            var transportWeb = new SendGrid.Web(Options.SendGridAPIKey);
            try
            {
                await transportWeb.DeliverAsync(sgMessage);    
            }
            catch (Exceptions.InvalidApiRequestException e)
            {
                _logger.LogDebug("{}", e);
                foreach (var error in e.Errors)
                {
                     _logger.LogDebug(error);
                }
            }
        }
    }
}