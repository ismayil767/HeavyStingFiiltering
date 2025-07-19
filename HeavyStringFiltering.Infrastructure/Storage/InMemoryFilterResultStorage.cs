using HeavyStringFiltering.Application.Interfaces;
using System.Collections.Concurrent;

namespace HeavyStringFiltering.Infrastructure.Storage
{
    public class InMemoryResultStore : IResultStore
    {
        private readonly ConcurrentDictionary<string, string> _results = new();

        public void Save(string uploadId, string result)
        {
            _results[uploadId] = result;
        }

        public bool TryGet(string uploadId, out string result)
        {
            return _results.TryGetValue(uploadId, out result);
        }
    }

}
