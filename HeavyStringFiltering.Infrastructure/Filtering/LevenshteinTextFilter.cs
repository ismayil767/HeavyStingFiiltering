using HeavyStringFiltering.Application.Interfaces;
using HeavyStringFiltering.Application.Settings;
using Microsoft.Extensions.Options;

namespace HeavyStringFiltering.Infrastructure.Filtering
{
    public class LevenshteinTextFilter : ITextFilter
    {
        private readonly List<string> _filterWords;
        private readonly double _similarityThreshold;

        public LevenshteinTextFilter(IOptions<FilterSettings> options)
        {
            _filterWords = options.Value.FilterWords;
            _similarityThreshold = options.Value.SimilarityThreshold;
        }

        public string Filter(string input)
        {
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var filtered = words.Where(word =>
                !_filterWords.Any(filter =>
                    CalculateSimilarity(word, filter) >= _similarityThreshold));

            return string.Join(" ", filtered);
        }

        private double CalculateSimilarity(string source, string target)
        {
            int distance = LevenshteinDistance(source, target);
            int maxLength = Math.Max(source.Length, target.Length);
            return maxLength == 0 ? 1.0 : 1.0 - (double)distance / maxLength;
        }

        private int LevenshteinDistance(string s, string t)
        {
            var dp = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++)
                dp[i, 0] = i;
            for (int j = 0; j <= t.Length; j++)
                dp[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1,     
                                 dp[i, j - 1] + 1),  
                                 dp[i - 1, j - 1] + cost);
                }
            }

            return dp[s.Length, t.Length];
        }
    }

}
