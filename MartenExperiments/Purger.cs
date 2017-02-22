using Marten;
using MartenExperiments.Models;
using MartenExperiments.Testing;
using Xunit;
using Xunit.Abstractions;

namespace MartenExperiments
{
    public class Purger : MartenTestBase
    {
        public Purger(ITestOutputHelper output) : base(output)
        {
        }

        //[Fact]
        [Fact(Skip = "Will Delete Data")]
        public void Clear_things_away()
        {
            using (var store = CreateDocumentStore(_ =>
            {
                _.AutoCreateSchemaObjects = AutoCreate.All;
                _.Events.InlineProjections.Add(new BasicCatalogViewProjection());
            }))
            {
                store.Advanced.Clean.CompletelyRemoveAll();
            }
        }
    }
}