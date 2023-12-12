using BlogAspNet;
using BlogAspNet.Data;
using BlogAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();
LoadConfiguration(app);

app.UseHttpsRedirection();
// pergunta quem voc� �
app.UseAuthentication();
// pergunta oque voc� pode fazer
app.UseAuthorization();
app.UseResponseCompression();
app.UseStaticFiles();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Estou em ambiente de desenvolvimento");
}
app.Run();


void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    //var smtp = new Configuration.SmtpConfiguration();
    //app.Configuration.GetSection("Smtp").Bind(smtp);
    //Configuration.Smtp = smtp;
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    // pegar chave 
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

    // adicionar metodo de autentica��o na aplica��o
    builder.Services.AddAuthentication(x =>
    {
        // selecionar padr�o de autentica��o como Jwt
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        // Quando autentica��o falha o padr�o de solicita��o � definido para Jwt
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        // cria��o dos parametros de valida��o do token
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // validar a chave de assinatura
            IssuerSigningKey = new SymmetricSecurityKey(key), // validar atraves de uma nova chave sim�trica 
            ValidateIssuer = false,
            ValidateAudience = false

        };
    });

}

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCompression(options => {
        options.Providers.Add<GzipCompressionProvider>();
    });
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Optimal;
    });
    builder
        .Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        });


}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<DataContext>();
    builder.Services.AddTransient<TokenService>(); // Sempre criar um novo tokenService
    //builder.Services.AddScoped(); | Reaproveita o TokenService se est� na mesma Requisi��o
    //builder.Services.AddSingleton(); | Utiliza o mesmo tokenService at� que a aplic~��o seja parada
    //builder.Services.AddTransient<EmailService>();


}
