using PhotoCache.Core.ReadModels;

namespace PhotoCache.Web.Models
{
    public interface IViewModel
    {
        UserModel CurrentUser { get; set; }
        object Model { get; set; }
    }
}