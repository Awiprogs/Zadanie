using System;

namespace Paari.Infrastructure.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product()
        {
            
        }

        public Product(string name, decimal price)
        {
            Price = price;
            Name = name;
        }

        public Product(Guid id, string name, decimal price) : this(name, price)
        {
            Id = id;
        }
    }
}
