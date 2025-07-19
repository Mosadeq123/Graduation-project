using Microsoft.AspNetCore.Identity;
using Store.G04.Core.Entities.Identity;
using System.Threading.Tasks;

namespace Store.G04.Core.Repositories.Contract
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager);

    }
}