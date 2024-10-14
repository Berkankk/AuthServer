using AuthServer.Core.DTOS;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController  //Temel CRUD işlemlerini yaptık
    {
        private readonly IGenericService<Product, ProductDto> _productService;

        public ProductController(IGenericService<Product, ProductDto> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var result = await _productService.GetAllAsync();
            return ActionResultInstance(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            var result = await _productService.AddAsync(productDto);
            return ActionResultInstance(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            var result = await _productService.Update(productDto, productDto.ProductID);
            return ActionResultInstance(result);
        }

        //Api/product/2 dedeiğimiz de böyle çalışacak
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var result = await _productService.Remove(id);
            return ActionResultInstance(result);
        }

    }
}
