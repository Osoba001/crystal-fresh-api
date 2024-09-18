using Share;

namespace Share.EmailService
{
    public interface IMailGenerator<TPayLoad>
    {
        Task<ActionResponse> SendAsync(TPayLoad payLoad);
    }
}
