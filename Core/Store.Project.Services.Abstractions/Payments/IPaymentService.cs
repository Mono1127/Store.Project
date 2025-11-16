using Store.Project.Shared.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Abstractions.Payments
{
    public interface IPaymentService
    {
        Task<BasketDto?> CreatePaymentIntentAsync(string basketId);
    }
}
