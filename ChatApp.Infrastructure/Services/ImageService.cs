using AutoMapper;
using ChatApp.Application.Common.Exceptions;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Common.Persistence;
using ChatApp.Application.Common.Settings;
using ChatApp.Domain.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ChatApp.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Cloudinary _cloudinary;

    public ImageService(IUnitOfWork unitOfWork, IOptions<CloudinarySettings> config, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cloudinary = new Cloudinary(new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret));
    }

    public async Task<ImageModel> AddImageAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            throw new ValidationException("Invalid file.");
        }

        ImageUploadResult uploadResult;

        await using (var stream = file.OpenReadStream())
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                AllowedFormats = new string[] { "png", "jpeg" }
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        if (uploadResult.Error is not null)
        {
            throw new ValidationException($"An error occurred while uploading an image: {uploadResult.Error.Message}");
        }

        var image = new Image()
        {
            Url = uploadResult.SecureUrl.AbsoluteUri,
            PublicId = uploadResult.PublicId
        };

        _unitOfWork.ImageRepository.Add(image);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ImageModel>(image);
    }

    public async Task DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

        if (deletionResult.Error != null)
        {
            throw new ValidationException($"An error occurred while deleting an image: {deletionResult.Error.Message}");
        }

        var image = (await _unitOfWork.ImageRepository.GetAsync()).FirstOrDefault(i => i.PublicId.Equals(publicId));

        if (image is null)
        {
            throw new ArgumentNullException(nameof(image), "Image with the specified public id doesn't exist.");
        }

        await _unitOfWork.ImageRepository.DeleteByIdAsync(image.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}