using System.Linq;
using FluentValidation;
using PhotoCache.Core.Persistence;
using PhotoCache.Core.ReadModels;
using PhotoCache.Validation;

namespace PhotoCache.Core.Validators
{
    public class UserModelValidator : AbstractMethodValidator<UserModel>
    {
        private IRavenRepository<UserModel> _users;

        public UserModelValidator(IRavenRepository<UserModel> users)
        {
            _users = users;
            
            RuleFor(x => x.UserName)
                .NotNull()
                .Length(6, 24)
                .Must(BeAnUnusedUserName)
                    .WithLocalizedMessage(() => Res.UserModel.UsernameExists);

            RuleFor(x => x.Password)
                .NotNull()
                .Length(6, 24);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }

        public bool BeAnUnusedUserName(string username)
        {
            switch(Method)
            {
                case ValidationMethod.Create:
                    return !_users.Query().Any(q => q.StoredUserName == username.ToLower());
                default:
                    return true;
            }
        }
    }
}