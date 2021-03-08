using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawLab.Models;
using Microsoft.AspNetCore.Identity;

namespace LawLab.Infrastructure
{
    public class LawLabUserValidator : UserValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = result.Succeeded ?
                new List<IdentityError>() : result.Errors.ToList();

            if (user.Email.ToLower().EndsWith("@example.com"))
            {
                errors.Add(new IdentityError
                {
                    Code = "SimpleEmailDomainError",
                    Description = "Допускаются только настоящие адреса электронной почты"
                });
            }

            return errors.Count == 0 ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());
        }
    }
}
