using System;
using System.Collections.Generic;
using PhotoCache.Core.ReadModels;
using Raven.Abstractions.Data;
using Raven.Client.Linq;

namespace PhotoCache.Core.Persistence
{
    public interface IRavenRepository<T> where T : IModel
    {
        IRavenQueryable<T> Query();
        T Load(Guid id);
        List<T> LoadAll();
        void Patch(Guid id, PatchRequest[] patchRequests);
        void Store(T resource);
        void Delete(T resource);
        void Delete(Guid id);
    }
}
