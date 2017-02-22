using System;
using FluentAssertions;
using TwentyTwenty.DomainDriven;

namespace MartenExperiments.Models
{
    public class Product
    {
        public string Id => Sku.ToString();
        public Sku Sku { get; private set; }

        public void Apply(ProductCreated @event)
        {
            Sku = @event.Sku;
        }
    }

    public class ProductCreated : IDomainEvent
    {
        public Sku Sku { get; }

        public ProductCreated(Sku sku)
        {
            Sku = sku;
        }

        public Guid Id { get; } = Guid.NewGuid();
    }
}