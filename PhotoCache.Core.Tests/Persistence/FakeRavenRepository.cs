using PhotoCache.Core.Persistence;
using PhotoCache.Core.ReadModels;
using Raven.Client;
using Raven.Client.Embedded;

namespace PhotoCache.Core.Specs.Persistence
{
    public class FakeRavenRepository<T> : RavenRepository<T> where T : IModel
    {
        public FakeRavenRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }
    }
}
