using HeavyStringFiltering.Application.Dtos;
using MediatR;

namespace HeavyStringFiltering.Application.Commands.UploadChunk
{
    public record UploadChunkCommand : IRequest<UploadChunkResponseDto>
    {
        public string UploadId { get; init; } = string.Empty;
        public int ChunkIndex { get; init; }
        public string Data { get; init; } = string.Empty;
        public bool IsLastChunk { get; init; }
    }
}
