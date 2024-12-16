namespace Pictogen.Application.ViewModels;

public sealed class ImageViewModel {
    public int ImageId { get; init; }
    public string? Url { get; init; }

    public ImageViewModel() { }

    public ImageViewModel(int imageId, string? url = "") {
        ImageId = imageId;
        Url = url;
    }
}
