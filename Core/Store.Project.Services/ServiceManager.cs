using AutoMapper;
using Store.Project.Domain.Contracts;
using Store.Project.Services.Abstractions;
using Store.Project.Services.Abstractions.Basket;
using Store.Project.Services.Abstractions.Products;
using Store.Project.Services.Basket;
using Store.Project.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork , IMapper _mapper,IBasketRepository basketRepository,ICacheRepository cacheRepository) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductServices(_unitOfWork,_mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository, _mapper);

        public ICacheService CacheService { get; } = new CacheService(cacheRepository);
    }
}
