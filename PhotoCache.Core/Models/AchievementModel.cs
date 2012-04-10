using System;

namespace PhotoCache.Core.Models
{
    public class AchievementModel : IModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public DateTime? UnlockedDate { get; set; }
    }
}