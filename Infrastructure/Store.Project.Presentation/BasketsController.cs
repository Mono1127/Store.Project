using Microsoft.AspNetCore.Mvc;
using Store.Project.Services.Abstractions;
using Store.Project.Shared.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager serviceManager) :ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasketById(String id)
        {
            var result = await serviceManager.BasketService.GetBasketAsync(id);
            return Ok (result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
           var result = await serviceManager.BasketService.UpdateBasketAsync(basketDto);
            return Ok (result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }

    }
}
