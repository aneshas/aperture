using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Example.Infrastructure;
using Aperture.Example.Projections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aperture.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    
                    // TODO - Extract these to extension methods
                    
                    // Use default Event stream from Aperture.Core
                    services.AddSingleton<IEventStore, EventStore>();
                    services.AddSingleton<IStreamEvents, PullEventStream>();
                    
                    // Use custom offset tracker TODO - Use tracker from sql package
                    services.AddSingleton<ITrackOffset, OffsetTracker>();
                    
                    // Add projections
                    services.AddSingleton<SciFiMoviesProjection>();
                    services.AddSingleton<CrimeMovieProjection>();
                });
    }
}