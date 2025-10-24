using AutoMapper;
using Store.Project.Domain.Contracts;
using Store.Project.Services.Abstractions;
using Store.Project.Services.Abstractions.Products;
using Store.Project.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork , IMapper _mapper) : IServiceManager
    {
        public IProductService productService { get; } = new ProductServices(_unitOfWork,_mapper);
    }
}
