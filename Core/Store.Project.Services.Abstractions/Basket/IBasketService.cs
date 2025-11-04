using Store.Project.Shared.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Abstractions.Basket
{
    public interface IBasketService
    {
        Task<BasketDto?> GetBasketAsync(string id );
        Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto);
        Task<bool> DeleteBasketAsync(string id);

    }
}
