using System;
using System.Collections.Generic;
using System.Globalization;

namespace PhotoCache.Core.Models
{
    public enum Roles
    {
        User = 1,
        Moderator = 2,
        Admin = 99
    }

    public class UserModel : IModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string StoredUserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles Role { get; set; }
        public DateTime LastLogin { get; set; }

        public CultureInfo CultureInfo { get; set; }
        public List<AchievementModel> Achievements { get; set; }

        public UserModel()
        {
            Achievements = new List<AchievementModel>();
        }
    }
}
