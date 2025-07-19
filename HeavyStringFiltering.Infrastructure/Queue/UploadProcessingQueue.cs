using HeavyStringFiltering.Application.Dtos;
using HeavyStringFiltering.Application.Interfaces;
using System.Collections.Concurrent;

namespace HeavyStringFiltering.Infrastructure.Queue
{
   
    public class UploadProcessingQueue : IQueueService
    {
        private readonly ConcurrentQueue<UploadTask> _queue = new();

        public Task EnqueueAsync(string uploadId, string text)
        {
            _queue.Enqueue(new UploadTask(uploadId, text));
            return Task.CompletedTask;
        }

        public bool TryDequeue(out UploadTask task) => _queue.TryDequeue(out task);
    }

}
