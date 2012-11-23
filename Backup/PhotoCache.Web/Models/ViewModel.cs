using PhotoCache.Core.ReadModels;

namespace PhotoCache.Web.Models
{
    public class ViewModel : IViewModel
    {
        public UserModel CurrentUser { get; set; }
        public object Model { get; set; }
    }
}