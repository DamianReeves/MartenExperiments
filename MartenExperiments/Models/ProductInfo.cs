using System;

namespace MartenExperiments.Models
{
    /// <summary>
    /// ProductInfo which also supports projection
    /// </summary>
    /// <remarks>Using the default aggregate projection mechanism requires a Guid.</remarks>
    public class ProductInfo
    {
        public Guid Id { get; set; }

        public string Sku { get; private set; }
        public string Title { get; private set; }


        public void Apply(ProductCreated @event)
        {
            Id = @event.Id;
            Sku = @event.Sku.ToString();
        }

        public void Apply(ProductTitleChanged @event)
        {
            Title = @event.Title;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Sku)}: {Sku}, {nameof(Title)}: {Title}";
        }
    }
}