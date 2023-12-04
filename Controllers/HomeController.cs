using Microsoft.AspNetCore.Mvc;
using MinhaApi.Data;
using MinhaApi.Models;

namespace MinhaApi.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/home")]
        public List<TodoModel> Get([FromServices] AppDbContext context)
        {
            
            return context.Todos.ToList();
        }

        [HttpPost("/home")]
        public TodoModel Post([FromServices] AppDbContext context, [FromBody]TodoModel todo)
        {
            context.Todos.Add(todo);
            context.SaveChanges();
            return todo;
            
        }

        [HttpGet("/home/{id:int}")]
        public TodoModel GetById([FromServices] AppDbContext context, [FromRoute] int id)
        {
            return context.Todos.FirstOrDefault(x => x.Id == id);
        }

        [HttpPut("/home/{id:int}")]
        public TodoModel Put([FromServices] AppDbContext context, [FromRoute] int id, [FromBody] TodoModel todo)
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

            return model;
        }

        [HttpDelete("/home/{id:int}")]
        public string Delete([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var model = context.Todos.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return "Tarefa não encontrada";
            }

            context.Todos.Remove(model);
            context.SaveChanges();

            return "Tarefa excluida com sucesso!";
        }



    }
}
