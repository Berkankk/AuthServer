﻿using AuthServer.Core.DTOS;
using AuthServer.Core.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Services.Mapping
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<UserAppDto , UserApp>().ReverseMap();
        }
    }
}
