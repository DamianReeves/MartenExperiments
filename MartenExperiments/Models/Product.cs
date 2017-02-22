using FluentAssertions;

namespace MartenExperiments.Models
{
    public class Product
    {
        public string Id => Sku.ToString();
        public Sku Sku { get; private set; }
        public string Title { get; private set; }


        public void Apply(ProductCreated @event)
        {
            Sku = @event.Sku;
        }

        public void Apply(ProductTitleChanged @event)
        {
            Title = @event.Title;
        }
    }
}