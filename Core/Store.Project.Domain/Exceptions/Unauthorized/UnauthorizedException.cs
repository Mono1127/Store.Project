using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Domain.Exceptions.Unauthorized
{
    public class UnauthorizedException() : Exception("You Are Not Authorized !!")
    {
    }
}
