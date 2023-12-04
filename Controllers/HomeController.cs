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
    }
}
