using BlogAspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public IActionResult Get([FromServices] DataContext context)
        {
            var categories = context.Categories.ToList();
            return Ok(categories);
        }

        [HttpGet("v2/categories")]
        public IActionResult Get2([FromServices] DataContext context)
        {
            var categories = context.Categories.ToList();
            return Ok(categories);
        }
    }
}
