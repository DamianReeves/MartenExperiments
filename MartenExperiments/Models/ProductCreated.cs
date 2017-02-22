using System;
using TwentyTwenty.DomainDriven;

namespace MartenExperiments.Models
{
    public class ProductCreated : IDomainEvent
    {
        public Sku Sku { get; }

        public ProductCreated(Sku sku)
        {
            Sku = sku;
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public class ProductTitleChanged : IDomainEvent
    {
        public string Title { get; }

        public ProductTitleChanged(string title)
        {
            Title = title;
        }
        public Guid Id { get; } = Guid.NewGuid();
    }
}