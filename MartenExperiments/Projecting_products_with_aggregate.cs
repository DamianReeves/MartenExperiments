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
        public async Task Can_project_events_to_product_using_AggregateStreamsWith(Guid skuGuid, string title)
        {
            var sku = Sku.Parse(skuGuid.ToString());

            Output.WriteLine("Sku: {0}", sku);

            using (var store = CreateDocumentStore(_ =>
            {
                _.AutoCreateSchemaObjects = AutoCreate.All;
                _.Events.InlineProjections.AggregateStreamsWith<ProductInfo>();
            }))
            {
                // Make changes to an aggregate
                var aggregate = ProductAggregate.Create(sku, skuGuid);
                aggregate.ChangeTitle(title);

                Output.WriteLine("Aggregate: {0}", aggregate);

                object[] events = aggregate.GetUncommittedChanges().ToArray();

                // Save the events
                using (var session = store.OpenSession())
                {
                    session.Events.StartStream<ProductAggregate>(aggregate.Id, events);
                    await session.SaveChangesAsync();
                }

                // Retrieve the projected view
                using (var session = store.OpenSession())
                {
                    var product = await session.LoadAsync<ProductInfo>(aggregate.Id);

                    Output.WriteLine("Retrieved Product: {0}", product);

                    product.ShouldBeEquivalentTo(new
                    {
                        Id = aggregate.Id,
                        Sku = sku.ToString(),
                        Title = title
                    }, opt => opt.ExcludingMissingMembers());

                }
            }
        }

        [Theory]
        [AutoData(Skip = "Trying custom aggregation with string field but not working")]
        public async Task Can_project_events_to_product_using_AggregationProjection(Guid skuGuid)
        {
            var sku = Sku.Parse(skuGuid.ToString());

            Output.WriteLine("Sku: {0}", sku);
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
                    session.Events.StartStream<ProductAggregate>(skuGuid, events);
                    await session.SaveChangesAsync();
                }

                using (var session = store.OpenSession())
                {
                    var product = await session.LoadAsync<Product>(sku.ToString());

                    product.ShouldBeEquivalentTo(new
                    {
                        Id = sku.ToString(),
                        Sku = sku
                    }, opt => opt.ExcludingMissingMembers());

                }
            }
        }        


    }
}