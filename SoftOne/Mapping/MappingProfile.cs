using AutoMapper;
using SoftOne.Entities;
using SoftOne.Models.Base;
using SoftOne.Models.Dtos;
using SoftOne.Models.Requests;

namespace SoftOne.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<string, byte[]>().ConvertUsing<RowVersionTypeConverter>();

        CreateMap<TaskItem, TaskDto>()
            .ForMember(
                dest => dest.RowVersion,
                opt => opt.MapFrom(src =>
                    src.RowVersion == null || src.RowVersion.Length == 0
                        ? null
                        : Convert.ToBase64String(src.RowVersion)));

        CreateMap<TaskAddRequest, TaskItem>().IncludeAllDerived().ReverseMap();

        CreateMap<BaseUpdateRequest, BaseEntity>()
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .IncludeAllDerived().ReverseMap();

        CreateMap<TaskUpdateRequest, TaskItem>()
            .IncludeBase<BaseUpdateRequest, BaseEntity>()
            .ForMember(
                dest => dest.IsReOpened,
                opt => opt.MapFrom(src => src.IsCompleted ? false : src.IsReOpened));

        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.RowVersion,
                opt => opt.MapFrom(src =>
                    src.RowVersion == null || src.RowVersion.Length == 0
                        ? null
                        : Convert.ToBase64String(src.RowVersion)));
    }
}
