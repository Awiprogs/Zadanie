using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Paari.Infrastructure.Model;
using Paari.Infrastructure.Repository;
using Paari.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Paari.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: api/product
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _repository.GetProducts();
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            Product item = _repository.GetById(id);
            if (item == null)
            {
                return NotFound("No product was found fo the given id");
            }

            return Ok(item);
        }

        // POST api/product
        [HttpPost]
        public IActionResult Post([FromBody]ProductCreateInputModel model)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be a null object");
            }

            Guid id = _repository.Create(new Product(model.Name, model.Price));

            return Ok(id);
        }

        // PUT api/product
        [HttpPut]
        public IActionResult Put([FromBody]ProductUpdateInputModel model)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be a null object");
            }

            try
            {
                _repository.Update(new Product(model.Id, model.Name, model.Price));
            }
            catch (ArgumentException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }

            return Ok();
        }

        // DELETE api/product/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch (ArgumentException exc)
            {
                return NotFound(exc.Message);
            }

            return Ok();
        }
    }
}
