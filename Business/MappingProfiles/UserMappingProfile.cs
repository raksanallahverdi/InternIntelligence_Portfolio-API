using AutoMapper;
using Business.Dtos.Achievement;
using Business.Dtos.Auth;
using Business.Dtos.ContactForm;
using Business.Dtos.Project;
using Business.Dtos.Skill;
using Business.Dtos.User;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class UserMappingProfile:Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Common.Entities.User, UserDto>();

            CreateMap<AuthRegisterDto, Common.Entities.User>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<CreateProjectDto,Common.Entities.Project>();
            CreateMap<UpdateProjectDto, Common.Entities.Project>();

            CreateMap<CreateSkillDto, Common.Entities.Skill>();
            CreateMap<UpdateSkillDto, Common.Entities.Skill>();

            CreateMap<CreateAchievementDto, Achievement>();
            CreateMap<UpdateAchievementDto, Achievement>();

            CreateMap<CreateContactFormDto, ContactForm>();

        }

    }
}
