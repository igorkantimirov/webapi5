using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication5.Controllers;
using WebApplication5.Data;
using WebApplication5.Helpers;
using WebApplication5.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(configuration =>
{
    configuration.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
    configuration.CustomSchemaIds(type => type.ToString());
    configuration.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme.\r\n\r\n" +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    configuration.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
});

var key = Encoding.ASCII.GetBytes(Enumerable.Range(1, 129).ToList().ToString()!); // todo: move to _appSettings // Encoding.ASCII.GetBytes(appSettings.Secret);
builder.Services
    .AddAuthentication(configuration =>
    {
        configuration.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        configuration.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(configuration =>
    {
        configuration.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
                var userId = context.Principal?.Identity?.Name;
                if (userId == null)
                {
                    context.Fail("Unauthorized");
                    return Task.CompletedTask;
                }

                var user = db.Users.AsNoTracking()
                    .FirstOrDefault(x => x.Id == Guid.Parse(userId));

                if (user == null)
                {
                    context.Fail("Unauthorized");
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            }
        };
        configuration.RequireHttpsMetadata = false;
        configuration.SaveToken = true;
        configuration.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();