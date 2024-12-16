using Microsoft.EntityFrameworkCore;
using Pictogen.Application.ViewModels;
using Pictogen.Infrastructure.Contexts;

namespace Pictogen.Application.Services;

public interface IImageService {
    Task<ImageViewModel?> GetImageByIdAsync(int id);
}

public sealed class ImageService : IImageService {
    private readonly IDbContextFactory<PictogenDbContext> _dbContextFactory;
    private readonly CancellationToken _cancellationToken;
    
    public ImageService(IDbContextFactory<PictogenDbContext> dbContextFactory, CancellationToken cancellationToken) {
        _dbContextFactory = dbContextFactory;
        _cancellationToken = cancellationToken;
    }

    public async Task<ImageViewModel?> GetImageByIdAsync(int id) {
        await using PictogenDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(_cancellationToken);
        
        return await dbContext.Images
                              .Where(image => image.Id == id)
                              .Select(image => new ImageViewModel(image.Id, image.Url))
                              .FirstOrDefaultAsync(_cancellationToken);
    }
}
