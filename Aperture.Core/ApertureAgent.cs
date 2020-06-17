using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core.Supervisors;
using Microsoft.Extensions.Logging;

namespace Aperture.Core
{
    public class ApertureAgent
    {
        private static ApertureAgent _instance;

        private IStreamEvents _eventStream;

        private ISuperviseProjection _projectionSupervisor;

        private IHandleApertureException _exceptionHandler;
        
        private ILogger _logger;

        private CancellationTokenSource _cts;

        private CancellationToken? _token;

        private readonly List<IProjectEvents> _projections = new List<IProjectEvents>();

        private ApertureAgent()
        {
            _cts = new CancellationTokenSource();
            _projectionSupervisor = new OneForAll();
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

        public ApertureAgent UseExceptionHandler(IHandleApertureException exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;

            return this;
        }

        public ApertureAgent UseLogger(ILogger logger)
        {
            _logger = logger;

            return this;
        }

        public ApertureAgent AddProjection(IProjectEvents projection)
        {
            _projections.Add(projection);

            return this;
        }

        public async Task StartAsync()
        {
            try
            {
                CheckStartupConditions();

                _logger?.LogInformation("Aperture agent starting...");

                var tasks = _projections
                    .Select(x => _projectionSupervisor.Run(_eventStream, x, HandleException, _token ?? _cts.Token));

                // We are choosing concurrency with potential parallelism instead of using Parallel
                var completedTask = await Task.WhenAny(tasks);
                await completedTask;
            }
            catch (Exception e)
            {
                _logger?.LogCritical(e, "Aperture agent encountered an unhandled exception.");
                HandleException(e);
                throw;
            }
        }

        private void CheckStartupConditions()
        {
            if(_eventStream == null)
                throw new ArgumentNullException($"No Event Stream Provided");
        }

        private void HandleException(Exception e)
        {
            _logger?.LogError(e, "Aperture agent encountered an exception.");

            try
            {
                _exceptionHandler?.HandleApertureException(e).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, $"Could not call external exception handler: {_exceptionHandler?.GetType().FullName}");
            }
        }

        public void Stop() => _cts.Cancel();
    }
}