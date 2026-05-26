using EcommerceStore.Data; //importa la carpeta donde esta AppDbContext.cs
using EcommerceStore.Middleware;
using EcommerceStore.Services.AuthService;
using EcommerceStore.Services.CategoriaService;
using EcommerceStore.Services.ProductoService;
using EcommerceStore.Services.UsuarioService;
using EcommerceStore.Services.CarritoService;
using EcommerceStore.Services.DireccionService;
using EcommerceStore.Services.PedidoService;
using EcommerceStore.Services.StorageService;
using EcommerceStore.Services.CuponService;
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
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>(); 
builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<IDireccionService, DireccionService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<ICuponService, CuponService>();

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
            ClockSkew = TimeSpan.Zero, //evita que los tokens expiren 5 mins tarde 
            RoleClaimType = "rol" 
        };
    });

builder.Services.AddAuthorization();

// CORS: permite requests desde el frontend de Vite en desarrollo
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",   // Vite dev server
                "http://localhost:4173"    // Vite preview
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
    await storageService.InitializeMainBucket();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("FrontendDev");

app.UseMiddleware<GlobalExceptionMiddleware>(); //middleware para manejar excepciones globales

app.UseAuthentication();

app.UseAuthorization(); //pregunta si tenes permiso 

app.MapControllers();

app.Run();
