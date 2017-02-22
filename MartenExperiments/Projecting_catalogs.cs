using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Marten;
using MartenExperiments.Models;
using MartenExperiments.Testing;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;

namespace MartenExperiments
{
    public class Projecting_catalogs : MartenTestBase
    {
        public Projecting_catalogs(ITestOutputHelper output):base(output)
        {
        }

        [Theory]
        [AutoData]
        public async Task Should_project_events_to_view(string catalogName, Guid catalogId)
        {
            using (var store = CreateDocumentStore(_ =>
            {
                _.AutoCreateSchemaObjects = AutoCreate.All;
                _.Events.InlineProjections.Add(new BasicCatalogViewProjection());
            }))
            {
                var catalogAggr = CatalogAggregate.Create(catalogName, catalogId);
                using (var session = store.OpenSession())
                {
                    var events = catalogAggr.GetUncommittedChanges().Cast<object>().ToArray();
                    session.Events.StartStream<CatalogAggregate>(catalogAggr.Id, events);

                    await session.SaveChangesAsync();
                }

                using (var session = store.OpenSession())
                {
                    var view = await session.LoadAsync<BasicCatalogView>(catalogId);

                    view.ShouldBeEquivalentTo(new
                    {
                        Id=catalogId,
                        Name=catalogName
                    }, opt=>opt.ExcludingMissingMembers());
                }
            }
        }

        
    }
}
