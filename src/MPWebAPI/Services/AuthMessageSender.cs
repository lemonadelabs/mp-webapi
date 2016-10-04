using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            // var sgMessage = new SendGridMessage();
            // sgMessage.AddTo(email);
            // sgMessage.From = new System.Net.Mail.MailAddress("mailer@merlinmail.lemonadelabs.io", "donotreply");
            // sgMessage.Subject = subject;
            // sgMessage.Text = message;
            // sgMessage.Html = message;
            // var credentials = new System.Net.NetworkCredential(Options.SendGridUser, Options.SendGridAPIKey);
            // var transportWeb = new SendGrid.Web(credentials);

            var sgMessage = new SendGridMessage();
            sgMessage.AddTo(email);
            sgMessage.From = new System.Net.Mail.MailAddress("mailer@merlinmail.lemonadelabs.io", "donotreply");
            sgMessage.Subject = subject;
            sgMessage.Text = message;
            sgMessage.Html = message;
            var sgClient = new SendGrid.Client(Options.SendGridAPIKey);
            //var response = await sgClient.Post("", );


            try
            {
                // await transportWeb.DeliverAsync(sgMessage);    
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