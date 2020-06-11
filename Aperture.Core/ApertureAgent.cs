using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public class ApertureAgent
    {
        private static ApertureAgent _instance;

        private IStreamEvents _eventStream;

        private ISuperviseProjection _projectionSupervisor;

        private CancellationTokenSource _cts;

        private CancellationToken? _token;

        private readonly ApertureConfiguration _configuration = new ApertureConfiguration();

        private readonly List<IProjectEvents> _projections = new List<IProjectEvents>();

        private ApertureAgent()
        {
            _cts = new CancellationTokenSource();
        }

        public static ApertureAgent Instance()
        {
            if (_instance == null)
                _instance = new ApertureAgent();

            return _instance;
        }

        public ApertureAgent UseEventStream(IStreamEvents streamEvents)
        {
            _eventStream = streamEvents;

            return this;
        }

        public ApertureAgent UseCancellationTokenSource(CancellationTokenSource cts)
        {
            _cts = cts;

            return this;
        }

        public ApertureAgent UseCancellationToken(CancellationToken token)
        {
            _token = token;

            return this;
        }

        public ApertureAgent UseSupervisor(ISuperviseProjection supervisor)
        {
            _projectionSupervisor = supervisor;

            return this;
        }

        public ApertureAgent AddProjection(IProjectEvents projection)
        {
            // TODO - ignore or throw if already running
            _projections.Add(projection);

            return this;
        }

        public ApertureAgent AddProjection<T>() where T : IProjectEvents
        {
            // TODO - Activate and then add to _projections

            return this;
        }

        public ApertureAgent Configure(Action<ApertureConfiguration> configFunc)
        {
            configFunc(_configuration);

            return this;
        }

        public async Task StartAsync()
        {
            // TODO - Validate all fields are set

            // TODO - Pass logger + exception handler to each supervisor
            var tasks = _projections
                .Select(x => _projectionSupervisor.Run(_eventStream, x, _token ?? _cts.Token));

            try
            {
                // We are choosing concurrency with potential parallelism instead of Parallel.Invoke
                await Task.WhenAny(tasks);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Stop()
        {
            _cts.Cancel();
        }
    }
}