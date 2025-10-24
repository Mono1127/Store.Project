using Store.Project.Services.Abstractions.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Abstractions
{
    public  interface IServiceManager
    {
        IProductService productService { get; }
    }
}
