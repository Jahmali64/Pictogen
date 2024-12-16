using Microsoft.AspNetCore.Mvc;
using Pictogen.Application.Services;
using Pictogen.Application.ViewModels;

namespace Pictogen.API.Controllers;

[ApiController]
public sealed class ImageController(IImageService imageService) : ControllerBase {
    
    [HttpGet("api/images/{id:int}")]
    public async Task<ActionResult<ImageViewModel>> GetImage(int id) {
        try {
            ImageViewModel? result = await imageService.GetImageByIdAsync(id);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
