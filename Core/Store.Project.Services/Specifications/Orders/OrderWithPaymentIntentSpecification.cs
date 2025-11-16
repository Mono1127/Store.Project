using Store.Project.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Specifications.Orders
{
    public class OrderWithPaymentIntentSpecification : BaseSpecifications< Guid,Order>
    {
        public OrderWithPaymentIntentSpecification(string paymentIntentId ) : base(O => O.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
