using ChatApp.Application.Common.Models;
using ChatApp.Application.Common.Models.Image;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Application.Common.Interfaces;

/// <summary>
/// Represents an image service.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Adds a new image.
    /// </summary>
    /// <param name="file">Image to add.</param>
    /// <returns>Created image.</returns>
    public Task<ImageModel> AddImageAsync(IFormFile file);

    /// <summary>
    /// Deletes an image with the specified id.
    /// </summary>
    public Task DeleteImageAsync(string publicId);
}