using Pictogen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Pictogen.Infrastructure.Contexts;

public class PictogenDbContext : DbContext {
    public DbSet<Image> Images { get; set; }
    
    public PictogenDbContext(DbContextOptions<PictogenDbContext> options) : base(options) { }
}
