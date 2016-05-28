using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using XinTuo.Finance.Models;
using System.Data;

namespace XinTuo.Finance.AutoMapperProfiles
{

    public class CompanyProfile : Profile
    {
        
        public CompanyProfile()
        {
            
            CreateMap<IDataReader,MRegion>();
            CreateMap<IDataReader, MCompany>();

            CreateMap<IDataReader, List<MRegion>>();
        }
    }
}