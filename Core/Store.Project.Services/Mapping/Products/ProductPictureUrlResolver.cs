using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Project.Domain.Entities.Products;
using Store.Project.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Mapping.Products
{
    public class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductResponse, string>
    {
        public string Resolve(Product source, ProductResponse destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl)) 
            { 
                return $"{configuration["BaseUrl"]}/{source.PictureUrl}";

            }
            return string.Empty;
        }
    }
}
