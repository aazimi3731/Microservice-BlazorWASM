using AutoMapper;
using MicroServices.Grpc;
using GenderModel = SharedModels.Entities.Gender;
using MaritalStatusModel = SharedModels.Entities.MaritalStatus;
using CountryModel = SharedModels.Entities.Country;
using JobCategoryModel = SharedModels.Entities.JobCategory;
using EmployeeModel = SharedModels.Entities.Employee;
using AutoMapper.Extensions.EnumMapping;
using Google.Protobuf.WellKnownTypes;

namespace MemberService.Models.Profiles
{
  public class EmployeeProfile : Profile
  {
    public EmployeeProfile()
    {
      CreateMap<DateTime, Timestamp>().ConvertUsing(x => Timestamp.FromDateTime(x.ToUniversalTime()));
      CreateMap<Timestamp, DateTime>().ConvertUsing(x => x.ToDateTime());

      CreateMap<GenderModel, Gender>().ConvertUsingEnumMapping(opt => opt.MapValue(GenderModel.Other, Gender.Other)).ReverseMap();
      CreateMap<MaritalStatusModel, MaritalStatus>().ConvertUsingEnumMapping(opt => opt.MapValue(MaritalStatusModel.Other, MaritalStatus.Other)).ReverseMap();
      CreateMap<CountryModel, Country>().ReverseMap();
      CreateMap<JobCategoryModel, JobCategory>().ReverseMap();
      CreateMap<EmployeeModel, Employee>().ReverseMap();
    }
  }
}
