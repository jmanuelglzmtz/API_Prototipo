using System.Collections.Generic;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IProductService
    {        
        IEnumerable<Product> GetAll();        
    }

    public class ProductService : IProductService
    {
        private DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Product> GetAll()
        {              
                      
            AppUtilities.tenantChange(_context);
            
            var product = _context.Product;

            AppUtilities.tenantClose(_context);
            
            return product;
        }

    }
}