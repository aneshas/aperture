using System;
using Microsoft.Extensions.DependencyInjection;

namespace Aperture.Core
{
    public static class ApertureServiceCollectionExtensions
    {
        public static void AddAperture(
            this IServiceCollection services,
            Func<ApertureAgent, ApertureAgent> configure = null)
        {
            var agent = ApertureAgentBuilder
                .CreateDefault();

            services.AddSingleton(
                ctx =>
                {
                    foreach (var projection in ctx.GetServices<IProjectEvents>())
                        agent.AddProjection(projection);

                    if (configure != null)
                        agent = configure(agent);

                    return agent
                        .UseEventStream(ctx.GetService<IStreamEvents>());
                });
        }
    }
}