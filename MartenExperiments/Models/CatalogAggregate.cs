using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwentyTwenty.DomainDriven;
using TwentyTwenty.DomainDriven.EventSourcing;

namespace MartenExperiments.Models
{
    public class CatalogAggregate : EventSourcingAggregateRoot<CatalogAggregate>
    {

        public CatalogAggregate()
        {            
        }

        protected CatalogAggregate(Guid id, string name)
        {
            ApplyChange(new CatalogCreated(id,name));
        }

        public string Name { get; private set; }

        protected virtual void Apply(CatalogCreated @event)
        {
            Id = @event.Id;
            Name = @event.Name;
        }

        public static CatalogAggregate Create(string name, Guid? id = null)
        {
            var catalogId = id ?? Guid.NewGuid();
            if (catalogId == Guid.Empty)
            {
                catalogId = Guid.NewGuid();
            }

            return new CatalogAggregate(catalogId,name);
        }

        public class CatalogCreated : IDomainEvent
        {
            public Guid Id { get; }
            public string Name { get; }

            public CatalogCreated(Guid id, string name)
            {
                Id = id;
                Name = name;
            }
        }
    }    
}
