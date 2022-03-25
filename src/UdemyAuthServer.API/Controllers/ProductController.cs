using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Entities;
using UdemyAuthServer.Core.Services;

namespace UdemyAuthServer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product, ProductDto> _productService;

        public ProductController(IGenericService<Product, ProductDto> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await _productService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            return ActionResultInstance(await _productService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto product)
        {
            return ActionResultInstance(await _productService.AddAsync(product));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto product)
        {
            return ActionResultInstance(await _productService.UpdateAsync(product.Id, product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultInstance(await _productService.RemoveAsync(id));
        }
    }
}