namespace HeavyStringFiltering.Application.Interfaces
{
    public interface IChunkStorage
    {
        void AddChunk(string uploadId, int chunkIndex, string data);
        bool TryAssemble(string uploadId, out string fullText);
        void Clear(string uploadId);
    }

}
