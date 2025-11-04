using AutoMapper;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities.Basket;
using Store.Project.Domain.Exceptions;
using Store.Project.Domain.Exceptions.Basket;
using Store.Project.Services.Abstractions.Basket;
using Store.Project.Shared.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Basket
{
    public class BasketService(IBasketRepository basketRepository,IMapper mapper) : IBasketService
    {
        public async Task<BasketDto?> GetBasketAsync(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            if (basket is null) throw new BasketNotFoundException(id);
            var result =mapper.Map<BasketDto>(basket);
            return result;
        }

        public async Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto)
        {
           var basket =  mapper.Map<CustomerBasket>(basketDto);
             basket = await basketRepository.UpdateBasketAsync(basket);
            if (basket is null) throw new BasketCreateOrUpdateRequestException();
            var result = mapper.Map<BasketDto>(basket);
            return result; 

        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
           var flag = await  basketRepository.DeleteBasketAsync(id);
            if (flag == false) throw new BasketDeleteBadRequestException();
            return flag; 
        }

      
    }
}
