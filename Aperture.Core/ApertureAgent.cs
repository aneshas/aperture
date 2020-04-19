using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public partial class ApertureAgent
    {
        private static ApertureAgent _instance;

        private IStreamEvents _eventStream;

        private ISuperviseProjection _projectionSupervisor;

        private CancellationTokenSource _cts;

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

        // Let caller handle where to run it eg. topshelf, hangfire job, net core worker service etc...
        public void Run()
        {
            // TODO - Validate all fields are set

            var actions = _projections
                .Select(p => (Action) (async () => await _projectionSupervisor.Run(_eventStream, p, _cts.Token)))
                .ToArray();

            try
            {
                Parallel.Invoke(actions);
            }
            catch (Exception e)
            {
                // TODO - Log
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