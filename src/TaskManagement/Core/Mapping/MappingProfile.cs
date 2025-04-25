using AutoMapper;
using TaskManagement.Core.Dtos;
using TaskManagement.Infrastructure.Entities;

namespace TaskManagement.Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskItemEntity, TaskItemDto>();
        CreateMap<TaskItemDto, TaskItemEntity>();
        CreateMap<TaskItemCreateDto, TaskItemEntity>();
    }
}