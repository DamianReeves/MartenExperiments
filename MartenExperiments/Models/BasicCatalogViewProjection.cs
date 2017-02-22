using Marten.Events.Projections;

namespace MartenExperiments.Models
{
    public class BasicCatalogViewProjection : ViewProjection<BasicCatalogView>
    {
        public BasicCatalogViewProjection()
        {
            ProjectEvent<CatalogAggregate.CatalogCreated>(
                (view, @event) =>
                {
                    view.Id = @event.Id;
                    view.Name = @event.Name;
                }
            );
        }
    }
}