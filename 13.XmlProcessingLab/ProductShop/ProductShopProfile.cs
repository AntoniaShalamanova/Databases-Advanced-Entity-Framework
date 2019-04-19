using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDto, User>();

            this.CreateMap<ImportProductDto, Product>();

            this.CreateMap<ImportCategoryDto, Category>();

            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

            this.CreateMap<User, ExportUserSoldProductDto>()
                .ForMember(x => x.SoldProducts,
                    y => y.MapFrom(s => Mapper.Map<List<ExportProductsDto>>(s.ProductsSold)));
        }
    }
}
