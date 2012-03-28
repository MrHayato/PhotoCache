using System;
using MongoDB.Bson.Serialization.Attributes;

namespace PhotoCache.Core.Models
{
    public class TestModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}
