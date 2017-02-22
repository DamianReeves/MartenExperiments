using System;
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
            ApplyChange(new ProductTitleChanged(sku.ToString()));
        }

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
            ApplyChange(new ProductTitleChanged(title));
        }

        protected virtual void Apply(ProductCreated @event)
        {
            Id = @event.Id;
            Sku = @event.Sku;
        }

        protected virtual void Apply(ProductTitleChanged @event)
        {
            // Note we don't always have to change internal state
        }

        public static ProductAggregate Create(Sku sku)
        {
            return new ProductAggregate(sku);
        }
    }
}