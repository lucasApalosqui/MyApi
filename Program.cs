using BlogAspNet.Data;
using BlogAspNet.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddDbContext<DataContext>();

builder.Services.AddTransient<TokenService>(); // Sempre criar um novo tokenService
//builder.Services.AddScoped(); // Reaproveita o TokenService se está na mesma Requisição
//builder.Services.AddSingleton(); // Utiliza o mesmo tokenService até que a aplic~ção seja parada
var app = builder.Build();

app.MapControllers();

app.Run();
