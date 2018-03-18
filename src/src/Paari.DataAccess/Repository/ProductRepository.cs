using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Paari.Infrastructure.Model;
using Paari.Infrastructure.Repository;

namespace Paari.DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.ProductItems;
        }

        public Product GetById(Guid id)
        {
            return _context.ProductItems.SingleOrDefault(t => t.Id == id);
        }

        public Guid Create(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException("Product cannot be a null object");
            }

            EntityEntry<Product> entry = _context.ProductItems.Add(product);
            _context.SaveChanges();

            return entry.Entity.Id;
        }

        public void Update(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("Product cannot be a null object");
            }

            if (Exist(product.Id))
            {
                Product existingProduct = _context.ProductItems.First(p => p.Id == product.Id);
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;

                _context.ProductItems.Update(existingProduct);

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"For a given product ID: {product.Id} no product can be found!");
            }
        }

        public void Delete(Guid id)
        {
            if (Exist(id))
            {
                _context.ProductItems.Remove(GetById(id));
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"For a given product ID: {id} no product can be found!");
            }
        }

        private bool Exist(Guid id)
        {
            return _context.ProductItems.Any(p => p.Id == id);
        }
    }
}
