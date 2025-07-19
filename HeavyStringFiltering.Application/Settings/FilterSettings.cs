namespace HeavyStringFiltering.Application.Settings
{
    public class FilterSettings
    {
        public List<string> FilterWords { get; set; } = new();
        public double SimilarityThreshold { get; set; } = 0.8;
    }

}
