using System;
using System.Collections.Generic;
using System.Threading;

namespace Aperture.Core
{
    public partial class ApertureAgent
    {
        private static ApertureAgent _instance;
        
        private ISupervisionStrategy _supervisionStrategy;

        private CancellationTokenSource _cts;

        private readonly ApertureConfiguration _configuration = new ApertureConfiguration();

        private readonly List<ApertureProjection> _projections = new List<ApertureProjection>();

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

        public ApertureAgent UseCancellationTokenSource(CancellationTokenSource cts)
        {
            _cts = cts;

            return this;
        }

        public ApertureAgent UseSupervisionStrategy(ISupervisionStrategy strategy)
        {
            _supervisionStrategy = strategy;

            return this;
        }

        public ApertureAgent AddProjection(ApertureProjection apertureProjection)
        {
            // TODO - ignore or throw if already running
            _projections.Add(apertureProjection);

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
            // provide cancellation token

            // Based on SupervisionStrategy:
            // if one projection fails - cancel all and restart
            // if one projection fails - restart it indefinitely
            // - handle projection failures here not in projection, let projection fail
            // - 


            // Return something else instead eg. IDisposable ?
            return this;
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        // Then individual supervision strategy should have it's own configuration options
    }
}