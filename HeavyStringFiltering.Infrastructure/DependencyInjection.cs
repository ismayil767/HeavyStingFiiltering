using HeavyStringFiltering.Application.Interfaces;
using HeavyStringFiltering.Infrastructure.Filtering;
using HeavyStringFiltering.Infrastructure.Queue;
using HeavyStringFiltering.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace HeavyStringFiltering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IQueueService, UploadProcessingQueue>();
            services.AddSingleton<IResultStore, InMemoryResultStore>();
            services.AddSingleton<ITextFilter, LevenshteinTextFilter>();
            services.AddSingleton<IChunkStorage, InMemoryChunkStorage>();
            return services;
        }
    }
}
