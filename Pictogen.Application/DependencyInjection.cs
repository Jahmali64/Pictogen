using Microsoft.Extensions.DependencyInjection;
using Pictogen.Application.Services;

namespace Pictogen.Application;

public static class DependencyInjection {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddScoped<IImageService, ImageService>();
        
        return services;
    }
}
