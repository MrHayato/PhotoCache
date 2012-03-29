using System;
using MongoDB.Bson.Serialization.Attributes;

namespace PhotoCache.Core.Models
{
    public class UserModel
    {
        [BsonId]
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
