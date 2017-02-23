using System;
using TwentyTwenty.DomainDriven;

namespace MartenExperiments.Models
{
    public class ProductCreated : DomainEvent
    {                
        public ProductCreated(Sku sku, Guid id)
        {            
            Id = id;
            Sku = sku;
        }

        public Sku Sku { get; }

    }

    public class ProductTitleChanged : DomainEvent
    {
        public string Title { get; }

        public ProductTitleChanged(string title)
        {
            Title = title;
        }
    }
}