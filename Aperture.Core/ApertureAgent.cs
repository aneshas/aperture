using System;
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

        public ApertureAgent UseProjectionSupervisor(ISuperviseProjection strategy)
        {
            _projectionSupervisor = strategy;

            return this;
        }

        public ApertureAgent AddProjection(IProjectEvents projection)
        {
            // TODO - ignore or throw if already running
            _projections.Add(projection);

            return this;
        }

        public ApertureAgent AddProjection<T>(T projection) where T : IProjectEvents
        {
            // TODO - Activate and then add to _projections
            
            return this;
        }
        
        public ApertureAgent Configure(Action<ApertureConfiguration> configFunc)
        {
            configFunc(_configuration);

            return this;
        }

        // TODO - How to best handle this
        public ApertureAgent Start()
        {
            // TODO - Validate all fields are set
            // Run projections in parallel tasks

            var tasks = _projections.Select(p => 
                _projectionSupervisor.Run(_eventStream, p, _cts.Token));

            Task.WhenAll(tasks); // This will block until cts is cancelled

            // Return something else instead eg. IDisposable ?
            return this;
        }

        public void Stop()
        {
            _cts.Cancel();
        }
    }
}