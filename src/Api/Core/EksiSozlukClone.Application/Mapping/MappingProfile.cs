using AutoMapper;
using EksiSozlukClone.Common.Models.Queries;
using EksiSozlukClone.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozlukClone.Core.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User,LoginUserViewModel>().ReverseMap();   
        }
      
       
    }
}
