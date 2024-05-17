using AutoMapper;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using Sqids;

namespace MyRecipeBook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEnconder;

    public AutoMapping(SqidsEncoder<long> idEnconder)
    {
        _idEnconder = idEnconder;

        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestRecipeJson, Domain.Entities.Recipe>()
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

        CreateMap<string, Domain.Entities.Ingredient>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));

        CreateMap<DishType, Domain.Entities.DishType>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

        CreateMap<RequestInstructionJson, Domain.Entities.Instruction>();
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();

        CreateMap<Domain.Entities.Recipe, ResponseRegiteredRecipeJson>()
            .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEnconder.Encode(source.Id)));
    }
}
