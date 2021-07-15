using AutoMapper;
using RepChecker.DtoModels;
using RepChecker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepChecker.MappingProfiles
{
    public class DomainToDto : Profile
    {
        public DomainToDto()
        {
            CreateMap<ApplicationUserModel, ApplicationUserModelDto>();
            CreateMap<ReputationModel, ReputationModelDto>();
            CreateMap<StandingModel, StandingModelDto>();
            CreateMap<Test, TestDto>();
        }
    }
}
