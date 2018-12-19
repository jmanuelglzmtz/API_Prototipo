using System;

namespace WebApi.Dtos
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public Guid TenantId { get; set; }
        public string ProductName { get; set; }
        public Decimal UnitPrice { get; set; }
        public Int16 UnitsInStock { get; set; }
        public Int16 UnitsOnOrder { get; set; }
        public Int16 ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}