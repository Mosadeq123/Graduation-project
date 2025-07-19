using Store.G04.Core.Dtos.Auth;

namespace Store.G04.Core.Services.Contract
{
    public interface IEmailServices
    {
        Task SendEmail(EmailDto email);
    }
}
