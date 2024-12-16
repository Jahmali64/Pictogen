using Pictogen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Pictogen.Infrastructure.Contexts;

public class PictogenDbContext(DbContextOptions<PictogenDbContext> options) : DbContext(options) {
    public DbSet<Image> Images { get; set; }
}
