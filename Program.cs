using EcommerceStore.Data; //importa la carpeta donde esta AppDbContext.cs
using EcommerceStore.Services.AuthService;
using EcommerceStore.Services.UsuarioService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//lee la cadena de conexion de appsettings.json y la guarda en esta variable
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//configura el dbcontext para que use mysql con pomelo
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUsuariosService, UsuarioService>(); //registra el servicio de usuarios para que pueda ser inyectado en los controladores
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
            ValidateIssuer = false, //en produccion ponerlo en true
            ValidateAudience = false, //en produccion ponerlo en true
            ClockSkew = TimeSpan.Zero //evita que los tokens expiren 5 mins tarde 
        };
    });

builder.Services.AddAuthorization();

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

app.UseAuthentication();

app.UseAuthorization(); //pregunta si tenes permiso 

app.MapControllers();

app.Run();
