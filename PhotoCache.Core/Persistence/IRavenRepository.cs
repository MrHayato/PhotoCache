using System;
using System.Collections.Generic;
using PhotoCache.Core.Models;
using Raven.Client.Linq;

namespace PhotoCache.Core.Persistence
{
    public interface IRavenRepository<T> where T : IModel
    {
        IRavenQueryable<T> Query();
        T Load(Guid id);
        List<T> LoadAll();
        void Store(T resource);
        void Delete(Guid id);
    }
}
