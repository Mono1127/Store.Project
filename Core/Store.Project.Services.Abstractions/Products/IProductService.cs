using Store.Project.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services.Abstractions.Products
{
    public interface IProductService
    {

        Task <IEnumerable<ProductResponse>> GetAllProductsAsync ();
    }
}
