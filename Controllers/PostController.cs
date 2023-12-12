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
        public async Task<IActionResult> GetAsync(
            [FromServices] DataContext context,
            [FromQuery]int page = 1, 
            [FromQuery] int pageSize = 25)
        {
            var count = await context.Posts.CountAsync();
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
                    LastUpdateDate = x.LastUpdateDate
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count,
                page,
                pageSize,
                posts
            }));
        }


        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> DetailsAsync(
        [FromRoute] int id,
        [FromServices] DataContext context)
        {
            try
            {
                var post = await context.Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .ThenInclude(x => x.Roles)
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (post == null)
                    return NotFound(new ResultViewModel<Category>("05GT6 - Conteúdo não encontrada"));

                return Ok(new ResultViewModel<Post>(post));

            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Post>("05X56 - Não foi possivel ler o Conteúdo!"));
            }
        }

        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetByCategoryAsync(
            [FromRoute] string category,
            [FromServices] DataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();
                var posts = await context.Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .Include(x => x.Category)
                    .Where(x => x.Category.Slug == category)
                    .Select(x => new ListPostsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts

                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Post>("05X96 - Não foi possivel ler o Conteúdo!"));
            }
        }
    }

}
