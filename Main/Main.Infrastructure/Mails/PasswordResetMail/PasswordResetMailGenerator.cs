using Share;
using Share.EmailService;
using System.Reflection;

namespace Main.Infrastructure.Mails.PasswordResetMail
{
    internal class PasswordResetMailGenerator : IMailGenerator<PasswordResetPayload>
    {
        public PasswordResetMailGenerator(IMailSender emailSender, DeploymentConfiguration deploymentConfig)
        {
            _emailSender = emailSender;
            DeploymentConfiguration = deploymentConfig;
            _htmlString = GetHtmlString();
        }

        private string GetHtmlString()
        {
            string htmlString = "";
            var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var pathToHtmlFile = Path.Combine(executableLocation, @"Mails/PasswordResetMail/PasswordResetMailBody.html");
            if (File.Exists(pathToHtmlFile))
            {
                htmlString = File.ReadAllText(pathToHtmlFile);
                htmlString = htmlString.Replace("[SupportTeamEmail]", DeploymentConfiguration.SupportTeamEmail);
                htmlString = htmlString.Replace("[LogoUrl]", DeploymentConfiguration.LogoUrl);
            }
            return htmlString;
        }

        private void ReplacePasswordResetUrl(PasswordResetPayload payload)
        {

            _htmlString = _htmlString.Replace("[PIN]", payload.RecoveryPin.ToString());

        }


        public async Task<ActionResponse> SendAsync(PasswordResetPayload payLoad)
        {
            if (!string.IsNullOrEmpty(_htmlString))
            {
                ReplacePasswordResetUrl(payLoad);
                var email = new EmailDto { To = [payLoad.Receiver], Body = _htmlString, Subject = "Crystal Fresh Passsword Recovery." };
                return await _emailSender.SendEmailAsync([email]);
            }
            return BadRequestResult("Html String is empty");
        }

        private readonly DeploymentConfiguration DeploymentConfiguration;
        private readonly IMailSender _emailSender;
        private string _htmlString;
    }
}
