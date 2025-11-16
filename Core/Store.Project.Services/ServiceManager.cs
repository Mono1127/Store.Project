using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities.Identity;
using Store.Project.Services.Abstractions;
using Store.Project.Services.Abstractions.Auth;
using Store.Project.Services.Abstractions.Basket;
using Store.Project.Services.Abstractions.Orders;
using Store.Project.Services.Abstractions.Payments;
using Store.Project.Services.Abstractions.Products;
using Store.Project.Services.Auth;
using Store.Project.Services.Basket;
using Store.Project.Services.Orders;
using Store.Project.Services.Payments;
using Store.Project.Services.Products;
using Store.Project.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork
        , IMapper _mapper
        ,IBasketRepository basketRepository
        ,ICacheRepository cacheRepository,
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> options,
        IConfiguration configuration) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductServices(_unitOfWork,_mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository, _mapper);

        public ICacheService CacheService { get; } = new CacheService(cacheRepository);

        public IAuthService AuthService { get; } = new AuthService(userManager, options,_mapper);

        public IOrderService OrderService {get; } = new OrderService(_unitOfWork,_mapper,basketRepository);

        public IPaymentService PaymentService { get; } = new PaymentService(basketRepository, _unitOfWork,configuration,_mapper);
    }
}
