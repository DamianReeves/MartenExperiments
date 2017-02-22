using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using Marten.Events.Projections;

namespace MartenExperiments.Models
{
    public class ProductAggregateFinder : IAggregationFinder<Product>
    {
        public Product Find(EventStream stream, IDocumentSession session)
        {
            var productCreated = stream.Events.Select(x => x.Data).OfType<ProductCreated>().Single();
            var product = stream.IsNew ? new Product() : session.Load<Product>(productCreated.Sku.ToString()) ?? new Product();
            product.Apply(productCreated);
            return product;
        }

        public async Task<Product> FindAsync(EventStream stream, IDocumentSession session, CancellationToken token)
        {
            var productCreated = stream.Events.Select(x => x.Data).OfType<ProductCreated>().Single();
            var product = stream.IsNew 
                ? new Product() 
                : await session.LoadAsync<Product>(productCreated.Sku.ToString(), token).ConfigureAwait(false) ?? new Product();
            product.Apply(productCreated);
            return product;
        }

        public async Task FetchAllAggregates(IDocumentSession session, EventStream[] streams, CancellationToken token)
        {
            var productIds = (
                from stream in streams
                from e in stream.Events
                let createdEvent = e.Data as ProductCreated
                where createdEvent != null
                select createdEvent.Sku.ToString()
            ).Distinct().ToArray();

            await session.LoadManyAsync<Product>(token, productIds).ConfigureAwait(false);
        }
    }
}