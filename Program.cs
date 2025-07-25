using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using log4net.Config;
using log4net;
using System.Reflection;
using LibrarySystem.Data;
using LibrarySystem.Repositories;
using LibrarySystem.Services;
using LibrarySystem.Services.Services;
using LibrarySystem.Repositories.Repositories;
using LibrarySystem.Repositories.Interfaces;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Middleware.Logs;
using LibrarySystem.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Cấu hình log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

var logger = LogManager.GetLogger(typeof(Program));
logger.Info("Application is starting...");


// Register the DbContext with MySQL support
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
logger.Info("Database context configured with MySQL.");

// Authentication and Authorization
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],


            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });
logger.Info("JWT Bearer authentication configured.");

builder.Services.AddAuthorization();
// End of Authentication and Authorization configuration

//Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Fill token type: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Register the repositories and services
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IBorrowService, BorrowService>();

builder.Services.AddScoped<ILoginRequestRepository, LoginRequestRepository>();
builder.Services.AddScoped<ILoginRequestService, LoginRequestService>();

var app = builder.Build();
logger.Info("Application has been built successfully.");


// Register the CORS policy
//app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Lifetime.ApplicationStopping.Register(() =>
{
    AsyncLogger.Log(LibrarySystem.Middleware.Logs.LogLevel.Info, "Application stopping...");
    AsyncLogger.Shutdown();  
});

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandling>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
