using FluentValidation;
using Nancy;
using Nancy.ModelBinding;
using PhotoCache.Core.Extensions;
using PhotoCache.Core.ReadModels;
using PhotoCache.Core.Services;
using PhotoCache.Web.Helpers;

namespace PhotoCache.Web.Modules.APIModules
{
    public class UserModule : BaseAPIModule
    {
        private IModelService<UserModel> _modelService;

        public UserModule(IModelService<UserModel> modelService)
        {
            _modelService = modelService;
            Post["/user/register"] = x => CreateNewUser();
            Get["/user/validate"] = x => Validate();
        }

        private Response Validate()
        {
            UserModel user = this.Bind<UserModel>();
            
            var result = _modelService.Validate(user, Queries);

            return !result.IsValid 
                ? Response.Error(result.Errors) 
                : new Response().Ok();
        }

        private Response CreateNewUser()
        {
            UserModel user = this.Bind<UserModel>();

            try
            {
                user.StoredUserName = user.UserName.ToLower();
                user.Password = (user.Password + user.Id).GetSha1Hash();
                _modelService.Create(user);
                return Response.AsJson(user).Created();
            } 
            catch (ValidationException e)
            {
                return Response.Error(e.Errors);
            }
        }
    }
}