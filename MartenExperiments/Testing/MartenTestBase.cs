using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marten;
using MartenExperiments.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace MartenExperiments.Testing
{
    public abstract class MartenTestBase : IDisposable
    {
        protected MartenTestBase(ITestOutputHelper output)
        {
            Output = output;
            ConnectionOptions = CreateOptions();            
        }        

        public ITestOutputHelper Output { get; }
        public MartenConnectionOptions ConnectionOptions { get; }

        protected virtual void Configure(IConfigurationBuilder builder)
        {
        }

        protected virtual void Configure(StoreOptions options, MartenConnectionOptions connectionOptions)
        {
            var connectionString = connectionOptions.ToConnectionString();
            options.Connection(connectionString);
        }

        protected IDocumentStore CreateDocumentStore(Action<StoreOptions> configure = null, MartenConnectionOptions connectionOptions = null)
        {
            connectionOptions = connectionOptions ?? ConnectionOptions;
            Output.WriteLine("Using Connection: {0}", connectionOptions.ToConnectionString());
            return Marten.DocumentStore.For(options =>
            {
                options.Connection(connectionOptions.ToConnectionString());
                options.Schema.Include<DocumentStorageRegistry>();

                Configure(options, connectionOptions);
                configure?.Invoke(options);
            });

        }

        protected MartenConnectionOptions CreateOptions()
        {
            var password = Environment.GetEnvironmentVariable("Development:MartenExperiments:ConnectionInfo:Password");
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Host","localhost" },
                    {"Database", "postgres" },
                    {"Username", "postgres" },
                    {"Password",  "postgres"}
                })
                .AddEnvironmentVariables("Development:MartenExperiments:ConnectionInfo");
            var config = builder.Build();
            var options = new MartenConnectionOptions();
            config.Bind(options);
            return options;
        }

        protected virtual void Dispose(bool disposing)
        {
            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
