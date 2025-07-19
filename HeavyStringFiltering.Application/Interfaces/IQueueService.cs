using HeavyStringFiltering.Application.Dtos;

namespace HeavyStringFiltering.Application.Interfaces
{
    public interface IQueueService
    {
        Task EnqueueAsync(string uploadId, string text);
        bool TryDequeue(out UploadTask task);
    }
}
