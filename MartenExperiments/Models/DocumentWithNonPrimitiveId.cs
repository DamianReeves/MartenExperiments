using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Baseline;
using Marten.Schema;
using Marten.Schema.Identity;
using StringlyTyped;

namespace MartenExperiments.Models
{
    public class DocumentWithNonPrimitiveId
    {
        public DocumentId Id { get; set; }
        public string Name { get; set; }
    }

    public class DocumentId : Stringly<Guid>
    {
        public DocumentId(string value) : base(value)
        {
        }
    }

    public class DocumentIdGenerationStrategy : IIdGeneration, IIdGenerator<DocumentId>
    {
        public IEnumerable<Type> KeyTypes { get; } = new[]
        {
            typeof(DocumentId)
        };

        public IIdGenerator<T> Build<T>(IDocumentSchema schema)
        {
            return this.As<IIdGenerator<T>>();
        }

        public DocumentId Assign(DocumentId existing, out bool assigned)
        {
            if (existing == null || existing.Result == Guid.Empty || String.IsNullOrWhiteSpace(existing.Value))
            {
                assigned = true;
                return new DocumentId(Guid.NewGuid().ToString());
            }

            assigned = false;
            return existing;

        }
    }
}