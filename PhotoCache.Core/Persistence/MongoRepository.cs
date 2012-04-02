using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace PhotoCache.Core.Persistence
{
    //Repository for resources associated with a specific account
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        readonly MongoDatabase _db;
        readonly MongoCollection<T> _docs;
        readonly MongoCollection<T> _deleted;

        public MongoRepository(MongoDatabase db) : this(db, SafeMode.True)
        {
        }

        public MongoRepository(MongoDatabase db, SafeMode safeMode)
        {
            _db = db;
            _docs = _db.GetCollection<T>(typeof(T).Name, safeMode);
            _deleted = _db.GetCollection<T>("Deleted" + typeof(T).Name, safeMode);
        }

        public IEnumerable<BsonDocument> MapReduce(IMongoQuery query, string mapFunction, string reduceFunction)
        {
            var map = new BsonJavaScript(mapFunction);
            var reduce = new BsonJavaScript(reduceFunction);
            var result = _docs.MapReduce(query, map, reduce);
            return result.InlineResults;
        }

        public void Create(T resource) { _docs.Insert(resource); }

        public void Update(T resource) { _docs.Save(resource); }

        public void CreateOrUpdate(T resource) { _docs.Save(resource); }

        public IQueryable<T> CreateQuery() { return _docs.AsQueryable<T>(); }

        public IQueryable<T> CreateQuery(IMongoQuery query) { return _docs.Find(query).AsQueryable(); }

        public IQueryable<T> CreateQueryDeleted() { return _deleted.AsQueryable<T>(); }

        public IQueryable<T> CreateQueryDeleted(IMongoQuery query) { return _deleted.Find(query).AsQueryable(); }

        public virtual T Get(dynamic id)
        {
            if (id == null || string.IsNullOrEmpty(id.ToString()))
                return null;
            return _docs.FindOne(new QueryDocument("_id", id));
        }

        public virtual List<T> GetAll()
        {
            return _docs.FindAll().ToList();
        }

        public void Update(dynamic id, IMongoUpdate update)
        {
            var builder = update as UpdateBuilder;
            if (builder != null)
            {
                if (builder.ToBsonDocument().Any())
                    throw new Exception("UpdateBuilder cannot be empty");
            }
            _docs.Update(new QueryDocument("_id", id), update);
        }

        public void UpdateMany(IMongoQuery query, IMongoUpdate update)
        {
            _docs.Update(query, update, UpdateFlags.Multi);
        }

        public void Delete(dynamic id)
        {
            _docs.Remove(new QueryDocument("_id", id));
            _deleted.Remove(new QueryDocument("_id", id));
        }

        public void SoftDelete(dynamic id)
        {
            dynamic doc = Get(id);
            _deleted.Insert(doc);
            _docs.Remove(new QueryDocument("_id", id));
        }

        public void Delete(IMongoQuery query)
        {
            if (CreateQuery(query).Any()) _docs.Remove(query, RemoveFlags.None);
            if (CreateQueryDeleted(query).Any()) _deleted.Remove(query, RemoveFlags.None);
        }

        public void SoftDelete(IMongoQuery query)
        {
            var docs = CreateQuery(query).ToList();
            if (!docs.Any()) return;
            _deleted.InsertBatch(docs);
            _docs.Remove(query, RemoveFlags.None);
        }
    }
}
