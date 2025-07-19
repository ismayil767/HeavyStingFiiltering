using HeavyStringFiltering.Application.Interfaces;
using System.Collections.Concurrent;

namespace HeavyStringFiltering.Infrastructure.Storage
{
    public class InMemoryChunkStorage : IChunkStorage
    {
        private readonly ConcurrentDictionary<string, SortedDictionary<int, string>> _storage = new();

        public void AddChunk(string uploadId, int chunkIndex, string data)
        {
            var chunks = _storage.GetOrAdd(uploadId, _ => new SortedDictionary<int, string>());
            lock (chunks)
            {
                chunks[chunkIndex] = data;
            }
        }

        public bool TryAssemble(string uploadId, out string fullText)
        {
            fullText = string.Empty;

            if (_storage.TryGetValue(uploadId, out var chunks))
            {
                lock (chunks)
                {
                    if (chunks.Count == 0)
                        return false;

                    fullText = string.Join("", chunks.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));
                    return true;
                }
            }

            return false;
        }

        public void Clear(string uploadId)
        {
            _storage.TryRemove(uploadId, out _);
        }
    }


}
