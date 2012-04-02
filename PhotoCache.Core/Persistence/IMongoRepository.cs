using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PhotoCache.Core.Persistence
{
    public interface IMongoRepository<T> where T : class
    {
        IEnumerable<BsonDocument> MapReduce(IMongoQuery query, string mapFunction, string reduceFunction);
        void Create(T resource);
        void Update(T resource);
        void CreateOrUpdate(T resource);

        /// <summary>
        /// Allows for querying the document repository
        /// </summary>
        IQueryable<T> CreateQuery();

        IQueryable<T> CreateQuery(IMongoQuery query);

        /// <summary>
        /// Allows for querying against the soft deleted documents
        /// </summary>
        IQueryable<T> CreateQueryDeleted();

        IQueryable<T> CreateQueryDeleted(IMongoQuery query);
        T Get(dynamic id);
        List<T> GetAll(); 
        void Update(dynamic id, IMongoUpdate update);

        /// <summary>
        /// Updates all documents that match the query
        /// </summary>
        void UpdateMany(IMongoQuery query, IMongoUpdate update);

        /// <summary>
        /// Deletes a document from the repository, will remove soft deleted documents as well
        /// </summary>
        void Delete(dynamic id);

        /// <summary>
        /// Soft deletes a document from the repository by moving it to a collection named 
        /// "Deleted" + DocumentTypeName
        /// </summary>
        void SoftDelete(dynamic id);

        void Delete(IMongoQuery query);
        void SoftDelete(IMongoQuery query);
    }
}