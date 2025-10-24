using AutoMapper;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities.Products;
using Store.Project.Services.Abstractions.Products;
using Store.Project.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Products
{
    public class ProductServices(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
           var products = await _unitOfWork.GetRepository<int,Product>().GetAllAsync();
            var result =  _mapper.Map<IEnumerable<ProductResponse>>(products);
            return result ;
        }
        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<int,Product>().GetAsync(id);
            var result = _mapper.Map<ProductResponse>(product);
            return result ;

        }
        public async Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<int, ProductBrand>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTypeResponse>>(brands);
            return result;
        }
        public async Task<IEnumerable<BrandTypeResponse>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<int, ProductType>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTypeResponse>>(types);
            return result;
        }

    }
}
