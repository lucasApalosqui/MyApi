using BlogAspNet.Data;
using BlogAspNet.Models;
using BlogAspNet.ViewModels;
using BlogAspNet.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync([FromServices] DataContext context)
        {
            var posts = await context.Posts
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    Author = $"{x.Author.Name} ({x.Author.Email})",
                    Category = x.Category.Name,
                    LastUpdateDate = x.LastUpdateDate.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return Ok(new ResultViewModel<List<ListPostsViewModel>>(posts));
        }
    }
}
