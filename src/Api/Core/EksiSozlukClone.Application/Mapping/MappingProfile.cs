using AutoMapper;
using EksiSozlukClone.Common.Models.Queries;
using EksiSozlukClone.Common.Models.RequestModels;
using EksiSozlukClone.Common.Models.RequestModels.Core.Application.Features.Commands.User.Create;
using EksiSozlukClone.Core.Domain.Models;

namespace EksiSozlukClone.Core.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User,LoginUserViewModel>().ReverseMap();
            CreateMap<CreateUserCommand, User>();
            CreateMap<UpdateUserCommand, User>();
        }
      
       
    }
}
