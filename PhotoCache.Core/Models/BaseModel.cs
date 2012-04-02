using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace PhotoCache.Core.Models
{
    public abstract class BaseModel : IModel
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }
    }

    public interface IModel
    {
        Guid Id { get; set; }
    }
}
