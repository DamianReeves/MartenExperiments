using TwentyTwenty.DomainDriven.EventSourcing;

namespace MartenExperiments.Models
{
    public class ProductAggregate : EventSourcingAggregateRoot<ProductAggregate>
    {
        public Sku Sku { get; private set; }
        public ProductAggregate()
        {            
        }

        public ProductAggregate(Sku sku)
        {
            ApplyChange(new ProductCreated(sku));
        }

        protected virtual void Apply(ProductCreated @event)
        {
            Id = @event.Id;
            Sku = @event.Sku;
        }

        public static ProductAggregate Create(Sku sku)
        {
            return new ProductAggregate(sku);
        }
    }
}