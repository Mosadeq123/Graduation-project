using Microsoft.AspNetCore.Identity;
using Store.G04.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Repositpory.IDentity
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count()==0)
            {
                var user = new AppUser()
                {
                    Email = "MohamedTarekSallam@gmail.com",
                    DisplayName = "Mohamed Tarek",
                    UserName = "Mohamed.Tarek",
                    PhoneNumber = "01026646321",
                    Address = new Address()
                    {
                        FName = "Mohamed",
                        LName = "sallam",
                        City = "ElShorouk",
                        Country = "Egypt",
                        Street = "Street Sidi Ezz El-Din",
                    }
                };
                await _userManager.CreateAsync(user, "$9fG!w2Lq@7zXr8*B");
            }
        }
    }
}
