using AutoMapper;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities.Orders;
using Store.Project.Domain.Entities.Products;
using Store.Project.Domain.Exceptions;
using Store.Project.Domain.Exceptions.BadRequest;
using Store.Project.Domain.Exceptions.Basket;
using Store.Project.Services.Abstractions.Orders;
using Store.Project.Services.Specifications.Orders;
using Store.Project.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail)
        {

           var orderAddress = _mapper.Map<OrderAddress>(request.ShipToAddress);

            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(request.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);

            var basket = await _basketRepository.GetBasketAsync(request.BasketId);
            if(basket is null) throw new BasketNotFoundException(request.BasketId);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
               var prodcut = await _unitOfWork.GetRepository<int,Product>().GetAsync(item.Id);
                if (prodcut == null) throw new ProductNotFoundException(item.Id);
                if(prodcut.Price != item.Price) item.Price = prodcut.Price;

                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);

                var orderItem = new OrderItem(productInOrderItem, item.Price, item.Quantity);
                orderItems.Add(orderItem); 
            } 
             var  subTotal = orderItems.Sum(i => i.Price * i.Quantity);


            var order = new Order(userEmail, orderAddress, deliveryMethod, orderItems, subTotal);

            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);
            var count = await _unitOfWork.SaveChangesAsync();
            if (count == 0) throw new CreateOrderBadRequestException();
            return _mapper.Map<OrderResponse>(order);


        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodsAsync()
        {
           var result = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(result);

        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid id, string userEmail)
        {
            var spec = new OrderSpecification(id, userEmail);
          var order = await _unitOfWork.GetRepository<Guid, Order>().GetAsync(spec);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse?>> GetOrderForSpecificUserAsync(string userEmail)
        {
            var spec = new OrderSpecification( userEmail);
            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<OrderResponse?>>(order);
        }
    }
}
