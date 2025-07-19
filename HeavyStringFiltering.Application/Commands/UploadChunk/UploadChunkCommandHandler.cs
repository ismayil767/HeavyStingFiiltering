using HeavyStringFiltering.Application.Dtos;
using HeavyStringFiltering.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HeavyStringFiltering.Application.Commands.UploadChunk
{
    public class UploadChunkHandler : IRequestHandler<UploadChunkCommand, UploadChunkResponseDto>
    {
        private readonly IChunkStorage _chunkStorage;
        private readonly IQueueService _queueService;
        private readonly ILogger<UploadChunkHandler> _logger;

        public UploadChunkHandler(
            IChunkStorage chunkStorage,
            IQueueService queueService,
            ILogger<UploadChunkHandler> logger)
        {
            _chunkStorage = chunkStorage;
            _queueService = queueService;
            _logger = logger;
        }

        public async Task<UploadChunkResponseDto> Handle(UploadChunkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _chunkStorage.AddChunk(request.UploadId, request.ChunkIndex, request.Data);
                _logger.LogInformation("Chunk received: {UploadId}, Index: {ChunkIndex}", request.UploadId, request.ChunkIndex);

                if (request.IsLastChunk)
                {
                    if (_chunkStorage.TryAssemble(request.UploadId, out var fullText))
                    {
                        if (Encoding.UTF8.GetByteCount(fullText) > 100 * 1024 * 1024) // 100 MB
                        {
                            throw new ValidationException("Total upload exceeds 100MB limit.");
                        }

                        await _queueService.EnqueueAsync(request.UploadId, fullText);
                        _logger.LogInformation("Final chunk received. Full string assembled and enqueued. UploadId: {UploadId}", request.UploadId);
                        _chunkStorage.Clear(request.UploadId);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to assemble chunks for UploadId: {UploadId}", request.UploadId);
                    }
                }

                return new UploadChunkResponseDto { Status = "Accepted" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chunk for UploadId: {UploadId}, Index: {ChunkIndex}", request.UploadId, request.ChunkIndex);
                return new UploadChunkResponseDto { Status = "Failed" };
            }
        }
    }

}
