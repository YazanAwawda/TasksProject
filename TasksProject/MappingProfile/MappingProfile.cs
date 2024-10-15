using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using TasksProject.Dto;
using TasksProject.Models;

namespace TasksProject.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoleDto, Roles>();   // Mapping from RoleDto to Role
            CreateMap<Roles, RoleDto>();   // If needed, mapping from Role to RoleDto

            CreateMap<UserDto, Users>();  
            CreateMap<Users, UserDto>();

            CreateMap<TaskDto, Tasks>();
            CreateMap<Tasks, TaskDto>();


            CreateMap<AssignTaskDto, Tasks>();
            CreateMap<Tasks, AssignTaskDto>();


            CreateMap<ChangeTaskStatusDto, Tasks>();
            CreateMap<Tasks, ChangeTaskStatusDto>();


            CreateMap<LoginDto, Users>();
            CreateMap<Users, LoginDto>();


            CreateMap<RegisterDto, Users>();
            CreateMap<Users, RegisterDto>();

            CreateMap<Users, UserResponse>();


        }
    }
}
