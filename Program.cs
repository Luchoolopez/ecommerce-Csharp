using EcommerceStore.Data; //importa la carpeta donde esta AppDbContext.cs
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//lee la cadena de conexion de appsettings.json y la guarda en esta variable
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//configura el dbcontext para que use mysql con pomelo
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


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

app.MapGet("/usuarios", async (EcommerceStore.Data.AppDbContext db) =>
{
    var usuarios = await db.Usuarios.ToListAsync(); //toListAsync == SELECT * FROM usuarios o .find()

    return Results.Ok(usuarios);
});

app.Run();
