using AutoMapper;
using myshop.Entities.ViewModels;
using myshop.Models;
using myShop.BLL.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Mapping
{
    internal class ProductProfile : Profile
    {

        public ProductProfile() {

            CreateMap<Product, ProductListDTO>()
                             .ForMember(dst => dst.CategoryName,
                             opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, Product>()
                           .ForMember(dest => dest.Img, opt => opt.Ignore());

        }
    }
}
