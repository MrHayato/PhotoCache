using System.Linq;
using FluentValidation;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;

namespace PhotoCache.Core.Validators
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        private IRavenRepository<UserModel> _users;
 
        public UserModelValidator(IRavenRepository<UserModel> users)
        {
            _users = users;
            
            RuleFor(x => x.UserName)
                .NotNull()
                .Length(4, 12)
                .Must(username => !_users.Query().Any(q => q.StoredUserName == username.ToLower()))
                    .WithLocalizedMessage(() => Res.UserModel.UsernameExists);

            RuleFor(x => x.Password)
                .Length(6, 24);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}