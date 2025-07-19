using HeavyStringFiltering.Infrastructure.Queue;

namespace HeavyStringFiltering.Infrastructure.Test
{
    public class UploadProcessingQueueTests
    {
        [Fact]
        public async Task EnqueueAsync_And_TryDequeue_ShouldWork()
        {
            // Arrange
            var queue = new UploadProcessingQueue();
            var uploadId = "test-id";
            var text = "test data";

            // Act
            await queue.EnqueueAsync(uploadId, text);

            // Assert
            var result = queue.TryDequeue(out var task);
            Assert.True(result);
            Assert.NotNull(task);
            Assert.Equal(uploadId, task.UploadId);
            Assert.Equal(text, task.Text);
        }

        [Fact]
        public void TryDequeue_WhenEmpty_ShouldReturnFalse()
        {
            // Arrange
            var queue = new UploadProcessingQueue();

            // Act
            var result = queue.TryDequeue(out var task);

            // Assert
            Assert.False(result);
            Assert.Null(task);
        }
    }
}
