using BlogAspNet;
using BlogAspNet.Data;
using BlogAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// pegar chave 
var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

// adicionar metodo de autenticação na aplicação
builder.Services.AddAuthentication(x =>
{
    // selecionar padrão de autenticação como Jwt
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    // Quando autenticação falha o padrão de solicitação é definido para Jwt
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    // criação dos parametros de validação do token
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // validar a chave de assinatura
        IssuerSigningKey = new SymmetricSecurityKey(key), // validar atraves de uma nova chave simétrica 
        ValidateIssuer = false,
        ValidateAudience = false

    };
});


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

Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

var smtp = new Configuration.SmtpConfiguration();
app.Configuration.GetSection("Smtp").Bind(smtp);
Configuration.Smtp = smtp;


// pergunta quem você é
app.UseAuthentication();
// pergunta oque você pode fazer
app.UseAuthorization();

app.MapControllers();

app.Run();
