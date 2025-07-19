using Bogus;
using HeavyStringFiltering.Application.Settings;
using HeavyStringFiltering.Infrastructure.Filtering;
using Microsoft.Extensions.Options;

namespace HeavyStringFiltering.Infrastructure.Test
{
    public class LevenshteinTextFilterTests
    {
        [Fact]
        public void Filter_RemovesSimilarWords()
        {
            // Arrange
            var settings = Options.Create(new FilterSettings
            {
                SimilarityThreshold = 0.8,
                FilterWords = new List<string> { "apple", "banana" }
            });

            var filter = new LevenshteinTextFilter(settings);
            var input = "apple aple applle banana bananna";

            // Act
            var result = filter.Filter(input);

            // Assert
            var resultWords = result.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Assert.DoesNotContain("apple", resultWords);
            Assert.DoesNotContain("aple", resultWords);
            Assert.DoesNotContain("applle", resultWords);
            Assert.DoesNotContain("banana", resultWords);
            Assert.DoesNotContain("bananna", resultWords);
        }

    }
}
