using HeavyStringFiltering.Application.Interfaces;

namespace FilteringBackgroundService
{
    public class FilteringBackgroundService : BackgroundService
    {
        private readonly IQueueService _queue;
        private readonly ITextFilter _filter;
        private readonly IResultStore _resultStore;
        private readonly ILogger<FilteringBackgroundService> _logger;

        public FilteringBackgroundService(IQueueService queue, ITextFilter filter, IResultStore resultStore, ILogger<FilteringBackgroundService> logger)
        {
            _queue = queue;
            _filter = filter;
            _resultStore = resultStore;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background filtering service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var task))
                {
                    _logger.LogInformation("Processing upload {UploadId}", task.UploadId);

                    var result = _filter.Filter(task.Text);
                    _resultStore.Save(task.UploadId, result);

                    _logger.LogInformation("Filtered text saved for {UploadId}", task.UploadId);
                }

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
