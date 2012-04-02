using FluentValidation;
using PhotoCache.Core.Models;

namespace PhotoCache.Core.Validators
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .Length(4, 12);

            RuleFor(x => x.Email)
                .EmailAddress();
        }
    }
}