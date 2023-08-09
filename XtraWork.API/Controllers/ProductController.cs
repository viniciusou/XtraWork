using Microsoft.AspNetCore.Mvc;
using XtraWork.API.Services;

namespace XtraWork.API.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _productService.GetAll(cancellationToken);
            return Ok(response);
        }
    }
}