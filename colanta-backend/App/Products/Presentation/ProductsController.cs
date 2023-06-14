using colanta_backend.App.Products.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace colanta_backend.App.Products.Presentation
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private UpToVtexNullProductsAndSkus renderNullProducts;
        public ProductsController(UpToVtexNullProductsAndSkus renderNullProducts)
        {
            this.renderNullProducts = renderNullProducts;
        }

        // POST api/<ValuesController>
        [HttpPost]
        [Route("renderNullProducts")]
        public ActionResult Post()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authorization);
                if(authorization.Equals("Jesing0408")) {
                    this.renderNullProducts.Invoke().Wait();
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch(Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}
