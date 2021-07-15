using AutoMapper;
using RepChecker.DtoModels;
using RepChecker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.MappingProfiles
{
    public class DtoToDomain : Profile
    {
        public DtoToDomain()
        {
            CreateMap<ApplicationUserModelDto, ApplicationUserModel>();
            CreateMap<ReputationModelDto, ReputationModel>();
            CreateMap<StandingModelDto, StandingModel>();
            CreateMap<TestDto, Test>();
        }
    }
}
