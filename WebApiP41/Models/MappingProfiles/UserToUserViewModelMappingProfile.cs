using AutoMapper;
using WebApiP41.Models;
using WebApiP41.Models.DTO;

namespace WebApiP41.Models.MappingProfiles;

public class UserToUserViewModelMappingProfile: Profile
{
    public UserToUserViewModelMappingProfile()
    {
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => NameAndEmail(src.Name, src.Email)))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ImageUrl(src.Image)));
    }

    private string ImageUrl(ImageFile? file )
    {
        if (file == null) { 
            return null;
        };
        return $"/uploads/{file.Filename}";
    }

    private string NameAndEmail(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            email = "no-email@example.com";
        }
        // Implement your email normalization logic here
        return $"{name} <{email.ToLowerInvariant()}>";
    }

}
