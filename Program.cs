using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StockAPI.context;
using StockAPI.Models;
using StockAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock API", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
          {
              new OpenApiSecurityScheme
              {
                  Reference = new OpenApiReference
                  {
                      Type=ReferenceType.SecurityScheme,
                      Id="Bearer"
                  }
              },
              new string[]{}
          }
    });
    }
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["secret:Issuer"],
        ValidAudience = builder.Configuration["secret:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["secret:key"])),
        ValidateLifetime = false,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed.");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Plantoys", policy =>
    {
        policy.WithOrigins("http://localhost:4200","https://sundevcat.github.io","https://ecomstock.vercel.app")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


builder.Services.AddDbContext<DatabaseContext>(option =>
    option.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        // new MySqlServerVersion(new Version(10, 4, 21))
        new MySqlServerVersion(new Version(8, 0, 39))
    ));
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<TokenServices>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<HashService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/auth", [AllowAnonymous] (HttpContext context) =>
{
    var id = context.User.FindFirst("id")?.Value;
    var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

    return Results.Ok($"Authenticated:{role} : {id}");
});

app.UseCors("Plantoys");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
