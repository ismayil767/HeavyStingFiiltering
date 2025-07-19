namespace HeavyStringFiltering.Application.Dtos
{
    public class UploadTask
    {
        public string UploadId { get; }
        public string Text { get; }

        public UploadTask(string uploadId, string text)
        {
            UploadId = uploadId;
            Text = text;
        }
    }
}
