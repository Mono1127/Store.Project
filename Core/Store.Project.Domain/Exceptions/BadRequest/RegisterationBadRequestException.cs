using Store.Project.Domain.Exceptions.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Domain.Exceptions.BadRequest
{
    public class RegisterationBadRequestException(List<string> errors) : BadRequestException(string.Join(", ", errors))
    {
    }
}
