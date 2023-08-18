using AutoMapper;
using XtraWork.API.Entities;
using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeResponse>();
            CreateMap<EmployeeRequest, Employee>();
            CreateMap<Title, TitleResponse>();
            CreateMap<TitleRequest, Title>();
        }
    }
}