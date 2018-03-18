using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paari.Controllers;
using Paari.DataAccess;
using Paari.DataAccess.Repository;
using Paari.Infrastructure.Model;
using Paari.Models;
using System;
using System.Collections.Generic;

namespace Paari.Tests
{
    [TestClass]
    public class ProductControllerTests
    {
        private ProductController _productController;

        [TestInitialize]
        public void SetUp()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
            optionsBuilder.UseInMemoryDatabase();
            var options = (DbContextOptions<ProductContext>)optionsBuilder.Options;
            ProductContext productContext = new ProductContext(options);

            ProductRepository repository = new ProductRepository(productContext);

            _productController = new ProductController(repository);
        }

        [TestMethod]
        public void GivenValidCreateInputModel_WhenPost_ThenCompletedSuccessfully()
        {
            IActionResult result = _productController.Post(new ProductCreateInputModel
            {
                Name = "X",
                Price = 22.2m
            });

            OkObjectResult objectResult = result as OkObjectResult;
            Guid id = (Guid)objectResult.Value;

            Assert.IsNotNull(objectResult);
            Assert.AreNotEqual(Guid.Empty, id);
        }

        [TestMethod]
        public void GivenInvalidCreateInputModel_WhenPost_ThenFailed()
        {
            IActionResult result = _productController.Post(null);

            BadRequestObjectResult objectResult = result as BadRequestObjectResult;

            Assert.IsNotNull(objectResult);
        }

        [TestMethod]
        public void GivenValidId_WhenDelete_ThenCompletedSuccessfully()
        {
            IActionResult result = _productController.Post(new ProductCreateInputModel
            {
                Name = "X",
                Price = 22.2m
            });

            OkObjectResult objectResult = result as OkObjectResult;
            Guid id = (Guid)objectResult.Value;

            IActionResult resultDelete = _productController.Delete(id);
            OkResult objectDeleteResult = resultDelete as OkResult;

            Assert.IsNotNull(resultDelete);
        }

        [TestMethod]
        public void GivenInvalidId_WhenDelete_ThenFailed()
        {
            IActionResult result = _productController.Delete(Guid.NewGuid());

            NotFoundObjectResult objectResult = result as NotFoundObjectResult;

            Assert.IsNotNull(objectResult);
        }

        [TestMethod]
        public void WhenGet_ThenCompletedSuccessfully()
        {
            IEnumerable<Product> result = _productController.Get();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GivenValidId_WhenGet_ThenCompletedSuccessfully()
        {
            ProductCreateInputModel productToCreate = new ProductCreateInputModel
            {
                Name = "X",
                Price = 22.2m
            };

            IActionResult result = _productController.Post(productToCreate);

            OkObjectResult objectResult = result as OkObjectResult;
            Guid id = (Guid)objectResult.Value;

            IActionResult getResult = _productController.Get(id);
            OkObjectResult objectGetResult = getResult as OkObjectResult;

            Product obtainedProduct = (Product)objectGetResult.Value;

            Assert.IsNotNull(objectGetResult);
            Assert.AreEqual(id, obtainedProduct.Id);
            Assert.AreEqual(productToCreate.Name, obtainedProduct.Name);
            Assert.AreEqual(productToCreate.Price, obtainedProduct.Price);
        }

        [TestMethod]
        public void GivenInvalidId_WhenGet_ThenFailed()
        {
            IActionResult result = _productController.Get(Guid.NewGuid());

            NotFoundObjectResult objectResult = result as NotFoundObjectResult;

            Assert.IsNotNull(objectResult);
        }

        [TestMethod]
        public void GivenValidUpdateInputModel_WhenUpdate_ThenCompletedSuccessfully()
        {
            ProductCreateInputModel productToCreate = new ProductCreateInputModel
            {
                Name = "X",
                Price = 22.2m
            };

            IActionResult result = _productController.Post(productToCreate);

            OkObjectResult objectResult = result as OkObjectResult;
            Guid id = (Guid)objectResult.Value;

            IActionResult updateResult = _productController.Put(new ProductUpdateInputModel
            {
                Id = id,
                Name = "newName",
                Price = 9.85m
            });
            OkResult objectUpdateResult = updateResult as OkResult;

            Assert.IsNotNull(objectUpdateResult);
        }

        [TestMethod]
        public void GivenInvalidUpdateInputModel_WhenUpdate_ThenFailed()
        {
            IActionResult updateResult = _productController.Put(new ProductUpdateInputModel
            {
                Id = Guid.NewGuid(),
                Name = "newName",
                Price = 9.85m
            });

            NotFoundObjectResult objectUpdateResult = updateResult as NotFoundObjectResult;

            Assert.IsNotNull(objectUpdateResult);
        }

        [TestMethod]
        public void GivenNullUpdateInputModel_WhenUpdate_ThenBadRequest()
        {
            IActionResult updateResult = _productController.Put(null);

            BadRequestObjectResult objectUpdateResult = updateResult as BadRequestObjectResult;

            Assert.IsNotNull(objectUpdateResult);
        }
    }
}