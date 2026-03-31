using EcommerceStore.Data; //importa la carpeta donde esta AppDbContext.cs
using EcommerceStore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//lee la cadena de conexion de appsettings.json y la guarda en esta variable
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//configura el dbcontext para que use mysql con pomelo
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUsuariosService, UsuarioService>(); //registra el servicio de usuarios para que pueda ser inyectado en los controladores


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
