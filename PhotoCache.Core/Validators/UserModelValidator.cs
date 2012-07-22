using System;
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
            foreach (var userModel in _users.LoadAll())
            {
                Console.WriteLine(userModel);
            }
            return !_users.Query().Any(q => q.StoredUserName == username.ToLower());
        }
    }
}