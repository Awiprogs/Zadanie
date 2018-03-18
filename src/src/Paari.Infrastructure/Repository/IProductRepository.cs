using System;
using System.Collections.Generic;
using Paari.Infrastructure.Model;

namespace Paari.Infrastructure.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();

        Product GetById(Guid id);

        Guid Create(Product product);

        void Update(Product product);

        void Delete(Guid id);
    }
}
