using AutoMapper;
using myshop.Entities.Models;
using myShop.BLL.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Mapping
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile() {

            CreateMap<CategoryDTO, Category>().ReverseMap();
        
        }
    }
}
