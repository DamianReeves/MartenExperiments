using Marten;
using MartenExperiments.Models;

namespace MartenExperiments.Configuration
{
    public class DocumentStorageRegistry : MartenRegistry
    {
        public DocumentStorageRegistry()
        {
            this.For<BasicCatalogView>()
                .Duplicate(x => x.Name);
        }
    }
}