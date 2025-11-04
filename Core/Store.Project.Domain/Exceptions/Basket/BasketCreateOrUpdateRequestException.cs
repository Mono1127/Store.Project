using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Domain.Exceptions.Basket
{
    public class BasketCreateOrUpdateRequestException() : 
        BadRequestException($"Invalid Operation When Create Or Update Basket !!")
    {

    }
}
