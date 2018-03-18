using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paari.Infrastructure.Model;
using System.Collections.Generic;
using Paari.DataAccess;
using Microsoft.EntityFrameworkCore;
using Paari.DataAccess.Repository;

namespace Paari.RepositoryTests
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private ProductRepository _productRepository;

        [TestInitialize]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
            optionsBuilder.UseInMemoryDatabase();
            var options = (DbContextOptions<ProductContext>)optionsBuilder.Options;
            ProductContext productContext = new ProductContext(options);

            _productRepository = new ProductRepository(productContext);
        }

        [TestMethod]
        public void Given_NotExistingGuid_When_GetById_ThenFailed()
        {
            Product product = _productRepository.GetById(Guid.NewGuid());

            Assert.IsNull(product);
        }

        [TestMethod]
        public void Given_ExistingGuid_When_GetById_Then_ProductReturned()
        {
            Product productToSave = new Product
            {
                Name = "Product",
                Price = 22.2m
            };

            Guid newProductGuid = _productRepository.Create(productToSave);
            Product savedProduct = _productRepository.GetById(newProductGuid);

            Assert.AreEqual(newProductGuid, savedProduct.Id);
            Assert.AreEqual(productToSave.Name, savedProduct.Name);
            Assert.AreEqual(productToSave.Price, savedProduct.Price);
        }

        [TestMethod]
        public void When_GetProducts_ThenListOfProductsReturned()
        {
            IEnumerable<Product> products = _productRepository.GetProducts();

            Assert.IsNotNull(products);
        }

        [TestMethod]
        public void Given_NullProduct_When_Create_ThenFailed()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _productRepository.Create(null));
        }

        [TestMethod]
        public void Given_ExistingGuid_When_Create_Then_ProductIdReturned()
        {
            Product productToSave = new Product
            {
                Name = "Product",
                Price = 22.2m
            };

            Guid newProductGuid = _productRepository.Create(productToSave);

            Assert.AreNotEqual(Guid.Empty, newProductGuid);
        }

        [TestMethod]
        public void Given_NullProduct_When_Update_ThenFailed()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _productRepository.Update(null));
        }

        [TestMethod]
        public void Given_NotExistingGuid_When_Update_Then_Failed()
        {
            Product productToSave = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product",
                Price = 22.2m
            };

            Assert.ThrowsException<ArgumentException>(() => _productRepository.Update(productToSave));
        }

        [TestMethod]
        public void Given_ExistingGuid_When_Update_Then_CompletedSuccessfully()
        {
            Product productToSave = new Product
            {
                Name = "Product",
                Price = 22.2m
            };

            Guid newProductId = _productRepository.Create(productToSave);

            Product productToUpdate = new Product
            {
                Id = newProductId,
                Name = "Product new name",
                Price = 5.34m
            };

            _productRepository.Update(productToUpdate);

            Product updatedProduct = _productRepository.GetById(newProductId);

            Assert.AreNotEqual(Guid.Empty, newProductId);
            Assert.AreEqual(productToUpdate.Id, updatedProduct.Id);
            Assert.AreEqual(productToUpdate.Name, updatedProduct.Name);
            Assert.AreEqual(productToUpdate.Price, updatedProduct.Price);
        }

        [TestMethod]
        public void Given_NotExistingGuid_When_Delete_Then_Failed()
        {
            Assert.ThrowsException<ArgumentException>(() => _productRepository.Delete(Guid.NewGuid()));
        }

        [TestMethod]
        public void Given_ExistingGuid_When_Delete_Then_CompletedSuccessfully()
        {
            Product productToSave = new Product
            {
                Name = "Product",
                Price = 22.2m
            };

            Guid newProductId = _productRepository.Create(productToSave);
            Product newProductAfterCreate = _productRepository.GetById(newProductId);

            _productRepository.Delete(newProductId);
            Product newProductAfterDelete = _productRepository.GetById(newProductId);

            Assert.IsNotNull(newProductAfterCreate);
            Assert.AreEqual(newProductId, newProductAfterCreate.Id);
            Assert.IsNull(newProductAfterDelete);
        }
    }
}