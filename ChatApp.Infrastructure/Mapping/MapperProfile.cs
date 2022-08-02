using AutoMapper;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Common.Models.Authorization;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Mapping;

/// <summary>
/// Represents an AutoMapper profile.
/// </summary>
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<SignUpModel, User>();
        CreateMap<Image, ImageModel>();
    }
}