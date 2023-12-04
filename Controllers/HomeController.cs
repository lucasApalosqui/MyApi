using Microsoft.AspNetCore.Mvc;
using MinhaApi.Data;
using MinhaApi.Models;

namespace MinhaApi.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/home")]
        public IActionResult Get([FromServices] AppDbContext context)
        {
            try
            {
                return Ok(context.Todos.ToList());
            }catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        [HttpPost("/home")]
        public IActionResult Post([FromServices] AppDbContext context, [FromBody]TodoModel todo)
        {
            try
            {
                context.Todos.Add(todo);
                context.SaveChanges();
                return Created($"/{todo.Id}", todo);
                
            }catch (Exception ex)
            {
                return BadRequest();
            }
            
            
        }

        [HttpGet("/home/{id:int}")]
        public IActionResult GetById([FromServices] AppDbContext context, [FromRoute] int id)
        {
            try
            {
                var todo = context.Todos.FirstOrDefault(x => x.Id == id)
                return Ok(todo);
            }catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        [HttpPut("/home/{id:int}")]
        public IActionResult Put([FromServices] AppDbContext context, [FromRoute] int id, [FromBody] TodoModel todo)
        {
            try
            {
                var model = context.Todos.FirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    return null;
                }

                model.Title = todo.Title;
                model.Done = todo.Done;

                context.Update(model);
                context.SaveChanges();

                return Ok(model);
            }catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        [HttpDelete("/home/{id:int}")]
        public IActionResult Delete([FromServices] AppDbContext context, [FromRoute] int id)
        {
            try
            {
                var model = context.Todos.FirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    return NotFound("Tarefa não encontrada");
                }

                context.Todos.Remove(model);
                context.SaveChanges();

                return Ok("Tarefa removida!");
            }catch(Exception ex)
            {
                return BadRequest();
            }
            
        }



    }
}
