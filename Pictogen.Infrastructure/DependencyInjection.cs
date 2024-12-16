using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Pictogen.Infrastructure.Contexts;

namespace Pictogen.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContextFactory<PictogenDbContext>(options => options.UseSqlite(configuration.GetConnectionString("PictogenDb")));
        
        return services;
    }
}
