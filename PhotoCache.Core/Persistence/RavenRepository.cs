using System;
using System.Collections.Generic;
using System.Linq;
using PhotoCache.Core.ReadModels;
using Raven.Abstractions.Commands;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Linq;

namespace PhotoCache.Core.Persistence
{
    public class RavenRepository<T> : IRavenRepository<T> where T : IModel
    {
        private IDocumentStore _documentStore;

        public RavenRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public IRavenQueryable<T> Query()
        {
            IRavenQueryable<T> query;

            using (var session = _documentStore.OpenSession())
            {
                query = session.Query<T>();
            }

            return query;
        }

        public T Load(Guid id)
        {
            T result;

            using (var session = _documentStore.OpenSession())
            {
                result = session.Load<T>(id);
            }

            return result;
        }

        public List<T> LoadAll()
        {
            List<T> results;

            using (var session = _documentStore.OpenSession())
            {
                results = session.Query<T>().ToList();
            }

            return results;
        }

        public void Patch(Guid id, PatchRequest[] patchRequests)
        {
            var _id = typeof(T).Name + "/" + id;
            _documentStore.DatabaseCommands.Patch(id.ToString(), patchRequests);
        }

        public void Store(T resource)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(resource);
                session.SaveChanges();
            }
        }

        public void Delete(T resource)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Delete(resource);
                session.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Advanced.Defer(new DeleteCommandData { Key = id.ToString() });
                session.SaveChanges();
            }   
        }
    }
}
