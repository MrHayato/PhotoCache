using System;
using System.Collections.Generic;
using FluentValidation;
using Nancy;
using Nancy.ModelBinding;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class UserModule : BaseAPIModule
    {
        private IRavenRepository<UserModel> _users;
        private IValidator<UserModel> _userValidator;

        public UserModule(IRavenRepository<UserModel> users, IValidator<UserModel> userValidator)
        {
            _users = users;
            _userValidator = userValidator;
            Post["/user/register"] = x => CreateNewUser();
            Get["/user/validate"] = x => Validate();
        }

        private Response Validate()
        {
            UserModel user = this.Bind<UserModel>();
            var queries = new List<string>();

            foreach (var query in Request.Query)
            {
                queries.Add(query);
            }

            if (user.UserName == null)
                user.UserName = "";

            var result = _userValidator.Validate(user, queries.ToArray());

            return !result.IsValid 
                ? Response.Error(result.Errors) 
                : new Response().Ok();
        }

        private Response CreateNewUser()
        {
            UserModel user = this.Bind<UserModel>();

            if (user.UserName == null)
                user.UserName = "";

            var result = _userValidator.Validate(user);

            if (!result.IsValid)
                return Response.Error(result.Errors);

            user.StoredUserName = user.UserName.ToLower();

            _users.Store(user);
            return Response.AsJson(user).Created();
        }
    }
}