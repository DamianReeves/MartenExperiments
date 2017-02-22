using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Marten;
using Marten.Schema.Identity;
using MartenExperiments.Models;
using MartenExperiments.Testing;
using Ploeh.AutoFixture.Xunit2;
using StringlyTyped;
using Xunit;
using Xunit.Abstractions;

namespace MartenExperiments
{
    public class Primitive_obsession_buster : MartenTestBase
    {
        public Primitive_obsession_buster(ITestOutputHelper output) : base(output)
        {
        }

        
        [Theory(Skip = "Not Supported")]
        [AutoData(Skip = "Not Supported")]
        public async Task Should_be_able_to_save_document_with_non_primitive_Id(Guid documentId, string documentName)
        {
            var id = new DocumentId(documentId.ToString());
            using (var store = CreateDocumentStore(_ =>
            {
                _.AutoCreateSchemaObjects = AutoCreate.All;
                _.Schema
                    .For<DocumentWithNonPrimitiveId>()
                    .IdStrategy(new DocumentIdGenerationStrategy());
            }))
            {                
                using (var session = store.OpenSession())
                {
                    var doc = new DocumentWithNonPrimitiveId
                    {
                        Id = id,
                        Name = documentName
                    };

                    var existingDoc = await session.LoadAsync<DocumentWithNonPrimitiveId>(doc.Id.ToString());
                    if (existingDoc != null)
                    {
                        Output.WriteLine($"Document: {existingDoc.Id} already exists!");
                    }

                    session.Store(doc);
                    await session.SaveChangesAsync();
                }

                using (var session = store.OpenSession())
                {
                    var retrievedDoc = await session.LoadAsync<DocumentWithNonPrimitiveId>(id.ToString());
                    retrievedDoc.ShouldBeEquivalentTo(new
                    {
                        Id = id,
                        Name = documentName
                    });
                }
                
            }
        }
    }
}