using PhotoCache.Core.Models;

namespace PhotoCache.Web.Models
{
    public interface IViewModel
    {
        UserModel CurrentUser { get; set; }
        object Model { get; set; }
    }
}