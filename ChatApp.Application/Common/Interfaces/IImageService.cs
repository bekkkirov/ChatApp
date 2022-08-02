using ChatApp.Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Application.Common.Interfaces;

public interface IImageService
{
    public Task<ImageModel> AddImageAsync(IFormFile file);

    public Task DeleteImageAsync(string publicId);
}