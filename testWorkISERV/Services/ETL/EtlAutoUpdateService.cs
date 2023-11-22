namespace testWorkISERV.Services.ETL
{
    public class EtlAutoUpdateService : IHostedService, IDisposable
    {
        private readonly IEtlService _etlService;
        private Timer? _timer = null;
        public EtlAutoUpdateService(IEtlService etlService)
        {
            _etlService = etlService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(autoUpdate, null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void autoUpdate(object? state)
        {
            _etlService.StartUpdate();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
