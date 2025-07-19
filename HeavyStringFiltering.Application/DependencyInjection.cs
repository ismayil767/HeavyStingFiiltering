using HeavyStringFiltering.Application.Commands.UploadChunk;
using Microsoft.Extensions.DependencyInjection;

namespace HeavyStringFiltering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadChunkHandler).Assembly));
            return services;
        }
    }
}
