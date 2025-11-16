using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities.Orders;
using Store.Project.Domain.Entities.Products;
using Store.Project.Domain.Exceptions;
using Store.Project.Domain.Exceptions.Basket;
using Store.Project.Services.Abstractions.Payments;
using Store.Project.Shared.Dtos.Basket;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Project.Domain.Entities.Products.Product;

namespace Store.Project.Services.Payments
{
    public class PaymentService(IBasketRepository _basketRepository,IUnitOfWork _unitOfWork, IConfiguration _configuration,IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto?> CreatePaymentIntentAsync(string basketId)
        {
          var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) throw new BasketNotFoundException(basketId);

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<int,Product>().GetAsync(item.Id);
                if (product == null) throw new ProductNotFoundException(item.Id);

                item.Price = product.Price;
            }
            var subTotal = basket.Items.Sum(item => item.Price * item.Quantity);


            if (!basket.DeliveryMethodId.HasValue) throw new DeliveryMethodNotFoundException(-1);

            var deliveryMethod = await _unitOfWork.GetRepository<int,DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod == null) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingCost = deliveryMethod.Price;

            var amount = subTotal + deliveryMethod.Price;


            StripeConfiguration.ApiKey = _configuration["StripeOptions:secretKey0"];

            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (basket.PaymentIntentId is null)
            {
                var paymentIntentCreateOptions = new PaymentIntentCreateOptions
                {
                    Amount = (long)amount * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                 paymentIntent = await paymentIntentService.CreateAsync(paymentIntentCreateOptions);
            }
            else
            {
                var paymentIntentUpdateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = (long)amount * 100,
                 
                };
                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, paymentIntentUpdateOptions);
            }

            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            basket = await _basketRepository.UpdateBasketAsync(basket,TimeSpan.FromDays(1));
            return _mapper.Map<BasketDto>(basket);

        }
    }
}
