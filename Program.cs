using ApiRestNetNoxun.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;  // <-- para configuración avanzada de Swagger
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Obtener el proveedor y conexiones desde configuración
string dbProvider = builder.Configuration["DatabaseProvider"];
string sqlServerConn = builder.Configuration.GetConnectionString("SqlServerConnection");
string mySqlConn = builder.Configuration.GetConnectionString("MySqlConnection");

if (string.IsNullOrEmpty(dbProvider))
{
    throw new Exception("Configura 'DatabaseProvider' en appsettings.json (SqlServer o MySql).");
}

if (dbProvider == "SqlServer")
{
    if (string.IsNullOrEmpty(sqlServerConn))
        throw new Exception("Cadena de conexión 'SqlServerConnection' no configurada.");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(sqlServerConn));
}
else if (dbProvider == "MySql")
{
    if (string.IsNullOrEmpty(mySqlConn))
        throw new Exception("Cadena de conexión 'MySqlConnection' no configurada.");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(mySqlConn, ServerVersion.AutoDetect(mySqlConn)));
}
else
{
    throw new Exception("Database provider no válido. Usa 'SqlServer' o 'MySql' en appsettings.json.");
}

// Agregar controladores
builder.Services.AddControllers();

// Swagger para documentación y UI interactiva
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Metadata del API
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mi API Rest - ApiRestNetNoxun",
        Version = "v1",
        Description = "API para gestión de usuarios, roles, procedimientos y datasets."
    });

    // Si quieres habilitar autenticación JWT en Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

    // Si tienes XML comments, puedes habilitarlos aquí:
    /*
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    */
});

// Configuración de autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Autorización
builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
        c.RoutePrefix = string.Empty; // Para acceder en raíz: http://localhost:5000/
    });
}

// Middleware autenticación/autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
