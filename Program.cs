using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Trazert_API.Database;
using Trazert_API.Endpoints;
using Trazert_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    var route = builder.Configuration.GetSection("Route").Value;
    options.AddPolicy("AllowAll",
        policy => policy.WithOrigins(route ?? "*")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
        );
});

var connectionString = builder.Configuration.GetConnectionString("SQL") ?? throw new InvalidOperationException("Connection string 'SQL' not found.");
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<TokenProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey(Constants.Token))
                {
                    context.Token = context.Request.Cookies[Constants.Token];
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<TokenBlackList>();

builder.Services.AddEndpoints(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization(); //TODO!: Agregar Roles de Usuarios

app.UseExceptionHandler(handler =>
{
    handler
        .Run(async context => await Results.Problem(
            detail: "An internal server error occurred.",
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Internal Server Error"
        )
        .ExecuteAsync(context));
});

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies[Constants.Token];
    var blacklist = context.RequestServices.GetRequiredService<TokenBlackList>();

    if (!string.IsNullOrEmpty(token) && blacklist.IsBlacklisted(token))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { message = "Token revoked" });
        return;
    }
    
    await next();
});

app.MapEndpoints();
app.UseHttpsRedirection();

await app.RunAsync();