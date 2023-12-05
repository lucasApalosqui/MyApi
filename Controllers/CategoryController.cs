using BlogAspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] DataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("v2/categories")]
        public async Task<IActionResult> GetAsync2([FromServices] DataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(categories);
        }
    }
}
