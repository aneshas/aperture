using System;
using Aperture.Core;
using Aperture.Example.Projections;
using Aperture.Polly;
using Microsoft.Extensions.DependencyInjection;

namespace Aperture.Example.Infrastructure
{
    public static class ApertureServiceConfig
    {
        public static void AddApertureAgent(this IServiceCollection services)
        {
            services.AddDefaultPullEventStream();
            services.AddOffsetTracker();
            services.AddProjections();

            services.AddSingleton(
                ctx => ApertureAgentBuilder
                    .CreateDefault()
                    .AddProjection(ctx.GetService<SciFiMoviesProjection>())
                    .AddProjection(ctx.GetService<CrimeMovieProjection>())
                    .UseRestartWithBackOffSupervision()
                    .UseEventStream(ctx.GetService<IStreamEvents>()));
        }

        private static void AddDefaultPullEventStream(this IServiceCollection services)
        {
            // Use default Event stream from Aperture.Core
            services.AddSingleton<IEventStore, EventStore>();
            services.AddSingleton<IStreamEvents>(
                ctx => new PullEventStream(
                    ctx.GetService<IEventStore>(), 
                    new PullEventStream.Config
                    {
                        BatchSize = 50,
                        PullInterval = TimeSpan.FromSeconds(2)
                    }));
        }

        private static void AddOffsetTracker(this IServiceCollection services)
        {
            // Use custom offset tracker TODO - Use tracker from sql package
            services.AddSingleton<ITrackOffset, OffsetTracker>();
        }

        private static void AddProjections(this IServiceCollection services)
        {
            services.AddSingleton<SciFiMoviesProjection>();
            services.AddSingleton<CrimeMovieProjection>();
        }
    }
}