namespace HeavyStringFiltering.Application.Interfaces
{
    public interface IResultStore
    {
        void Save(string uploadId, string result);
        bool TryGet(string uploadId, out string result);
    }
}
