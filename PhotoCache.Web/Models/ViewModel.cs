using PhotoCache.Core.Models;

namespace PhotoCache.Web.Models
{
    public class ViewModel : IViewModel
    {
        public UserModel CurrentUser { get; set; }
        public object Model { get; set; }
    }
}