using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Marten;
using Marten.Events.Projections;
using MartenExperiments.Models;
using MartenExperiments.Testing;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;

namespace MartenExperiments
{
    public class Projecting_products_with_aggregate : MartenTestBase
    {
        public Projecting_products_with_aggregate(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [AutoData]
        public async Task Should_project_events_to_product(Guid skuGuid)
        {
            var sku = Sku.Parse(skuGuid.ToString());

            Output.WriteLine("Sku: {0}",sku);
            var projection = new AggregationProjection<Product>(
                new ProductAggregateFinder(), 
                new Aggregator<Product>());

            using (var store = CreateDocumentStore(_ =>
            {
                _.AutoCreateSchemaObjects = AutoCreate.All;
                //_.Events.InlineProjections.AggregateStreamsWith<Product>();
                _.Events.InlineProjections.Add(projection);
            }))
            {
                var aggregate = ProductAggregate.Create(sku);
                object[] events = aggregate.GetUncommittedChanges().ToArray();

                using (var session = store.OpenSession())
                {
                    session.Events.StartStream<Product>(skuGuid, events);
                    await session.SaveChangesAsync();
                }

                using (var session = store.OpenSession())
                {
                    var product = session.LoadAsync<Product>(sku.ToString());

                    product.ShouldBeEquivalentTo(new
                    {
                        Id = sku.ToString(),
                        Sku = sku
                    }, opt=>opt.ExcludingMissingMembers());

                }
            }
        }


    }
}