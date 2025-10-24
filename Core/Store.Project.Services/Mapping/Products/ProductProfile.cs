using AutoMapper;
using Store.Project.Domain.Entities.Products;
using Store.Project.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(D => D.Brand,O => O.MapFrom(S => S.Brand.Name))
                .ForMember(D => D.Type, O => O.MapFrom(S => S.Type.Name));

            CreateMap<ProductBrand, BrandTypeResponse>();
            CreateMap<ProductType, BrandTypeResponse>();
        }
    }
}
