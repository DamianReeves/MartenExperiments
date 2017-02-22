using System;
using System.Collections.Generic;

namespace MartenExperiments.Models
{
    public class BasicCatalogView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public void Apply(CatalogAggregate.CatalogCreated e)
        {
            Id = e.Id;
            Name = e.Name;
        }
    }     
}