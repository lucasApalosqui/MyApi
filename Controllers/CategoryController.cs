using BlogAspNet.Data;
using BlogAspNet.Models;
using BlogAspNet.ViewModels;
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
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "05T45 - Não foi possivel Ler as categorias!");
            }

        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] DataContext context, [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if(category == null)
                {
                    return NotFound("Categoria não encontrada");
                }
                return Ok(category);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "05X16 - Não foi possivel ler a categoria!");
            }

        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromServices] DataContext context, [FromBody] CreateCategoryViewModel model)
        {
            try
            {
                var category = new Category { Id = 0, Name = model.Name, Slug = model.Slug.ToLower() };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "05XE9 - Não foi possivel incluir a categoria!");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "05X10 - Não foi possivel incluir a categoria!");
            }

        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices] DataContext context, [FromBody] Category model, [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return NotFound("Categoria não encontrada");
                }

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Update(category);
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "05XE20 - Não foi possivel atualizar a categoria!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "05X15 - Não foi possivel atualizar a categoria!");
            }

        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] DataContext context, [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return NotFound("Categoria não encontrada");
                }

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok("Excluido com sucesso!");
               
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "05YE20 - Não foi possivel excluir a categoria!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "05k15 - Não foi possivel excluir a categoria!");
            }

        }

    }
}
