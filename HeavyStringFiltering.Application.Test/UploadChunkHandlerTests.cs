using Bogus;
using HeavyStringFiltering.Application.Commands.UploadChunk;
using HeavyStringFiltering.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace HeavyStringFiltering.Application.Test
{
    public class UploadChunkHandlerTests
    {
        private readonly Mock<IChunkStorage> _chunkStorageMock = new();
        private readonly Mock<IQueueService> _queueServiceMock = new();
        private readonly Mock<ILogger<UploadChunkHandler>> _loggerMock = new();

        [Fact]
        public async Task Handle_ShouldAddChunk_AndReturnAccepted()
        {
            // Arrange
            var faker = new Faker();
            var command = new UploadChunkCommand
            {
                UploadId = faker.Random.Guid().ToString(),
                ChunkIndex = 0,
                Data = faker.Lorem.Paragraph(),
                IsLastChunk = false
            };

            var handler = new UploadChunkHandler(
                _chunkStorageMock.Object,
                _queueServiceMock.Object,
                _loggerMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            _chunkStorageMock.Verify(x => x.AddChunk(command.UploadId, command.ChunkIndex, command.Data), Times.Once);
            Assert.Equal("Accepted", result.Status);
        }

        [Fact]
        public async Task Handle_WhenIsLastChunk_AndTryAssembleSucceeds_ShouldEnqueue()
        {
            // Arrange
            var faker = new Faker();
            var command = new UploadChunkCommand
            {
                UploadId = faker.Random.Guid().ToString(),
                ChunkIndex = 1,
                Data = faker.Lorem.Paragraph(),
                IsLastChunk = true
            };

            var assembledText = faker.Lorem.Paragraphs(2);
            _chunkStorageMock.Setup(x => x.TryAssemble(command.UploadId, out assembledText)).Returns(true);

            var handler = new UploadChunkHandler(
                _chunkStorageMock.Object,
                _queueServiceMock.Object,
                _loggerMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            _queueServiceMock.Verify(x => x.EnqueueAsync(command.UploadId, assembledText), Times.Once);
            _chunkStorageMock.Verify(x => x.Clear(command.UploadId), Times.Once);
            Assert.Equal("Accepted", result.Status);
        }
    }
}
