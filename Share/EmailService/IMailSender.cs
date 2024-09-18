using Share;

namespace Share.EmailService
{
    public interface IMailSender
    {
        [Obsolete("This method is deprecated. Kindly use SendAsync(List<Email> emails) instead.")]
        Task<bool> SendEmailAsync(EmailDto command);
        Task<ActionResponse> SendEmailAsync(List<EmailDto> emails);
    }
}
