using System;
using System.Collections.Generic;

namespace PhotoCache.Core.Models
{
    public class CacheModel : IModel
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastFound { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
