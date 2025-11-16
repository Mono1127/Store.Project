using Store.Project.Services.Abstractions.Auth;
using Store.Project.Services.Abstractions.Basket;
using Store.Project.Services.Abstractions.Orders;
using Store.Project.Services.Abstractions.Payments;
using Store.Project.Services.Abstractions.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Abstractions
{
    public  interface IServiceManager
    {
        IProductService ProductService { get; }
        IBasketService BasketService { get;  }
        ICacheService CacheService { get; }
        IAuthService AuthService { get; }
        IOrderService OrderService { get; }
        IPaymentService PaymentService { get; }
    }
}
