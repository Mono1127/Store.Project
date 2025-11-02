using Store.Project.Domain.Entities.Products;
using Store.Project.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Specifications.Products
{
    public class ProductsWithBrandAndTypeSpecifications : BaseSpecifications<int, Product>
    {
        public ProductsWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }

        public ProductsWithBrandAndTypeSpecifications(ProductQueryParameters parameters) : base
            (
                P => (!parameters.BrandId.HasValue || P.BrandId == parameters.BrandId) && (!parameters.TypeId.HasValue || P.TypeId == parameters.TypeId)
                &&
                (string.IsNullOrEmpty(parameters.Search) || P.Name.ToLower().Contains(parameters.Search.ToLower()))

            )
            
        {
            if (!string.IsNullOrEmpty(parameters.Sort)) 
            {
                switch (parameters.Sort.ToLower()) 
                {
                    case "priceasc" : AddOrderBy(P => P.Price);
                    break;
                    case "pricedesc": AddOrderByDescending (P => P.Price);
                    break;
                    default:AddOrderBy(P => P.Name);
                    break;
                }


            }
            else 
            {
                //OrderBy = P => P.Name; 
                AddOrderBy(P => P.Name);
            }


                ApplyPagination(parameters.PageSize, parameters.PageIndex);

                ApplyIncludes();

        }
        private void ApplyIncludes() 
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }

    }
}
