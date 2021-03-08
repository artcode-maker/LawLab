using LawLab.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Infrastructure
{
    public class LawLabPasswordValidator : PasswordValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            IdentityResult result = await base.ValidateAsync( manager, user, password);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>()
                : result.Errors.ToList();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Пароль не может включать имя пользователя"
                });
            }
            if (password.Contains("123456"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsNumbersSequence",
                    Description = "Пароль не может состоять только из последовательности чисел 123456"
                });
            }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
