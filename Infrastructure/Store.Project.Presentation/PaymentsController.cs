using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Store.Project.Services.Abstractions;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            var result = await _serviceManager.PaymentService.CreatePaymentIntentAsync(basketId);
            return Ok(result);
        }

        [Route("webhook")]
        [HttpPost]
            public async Task<IActionResult> Index()
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                const string endpointSecret = "whsec_...";
                
                    var stripeEvent = EventUtility.ParseEvent(json);
                    var signatureHeader = Request.Headers["Stripe-Signature"];

                    stripeEvent = EventUtility.ConstructEvent(json,
                            signatureHeader, endpointSecret);

                    // If on SDK version < 46, use class Events instead of EventTypes
                    if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                    {
                      
                    }
                    else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                    {
                      
                    }
                    else
                    {
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    }
                    return Ok();
           
            }
        
    }

}

