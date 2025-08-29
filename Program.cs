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
    options.AddPolicy("AllowAll",
        builder => builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

var cnstr = builder.Configuration.GetConnectionString("SQL") ?? throw new InvalidOperationException("Connection string 'SQL' not found.");
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(cnstr));

builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<TokenProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
                if (context.Request.Cookies.ContainsKey("trazert_jwt"))
                {
                    context.Token = context.Request.Cookies["trazert_jwt"];
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

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
    handler.Run(async context => await Results.Problem().ExecuteAsync(context));
});

app.MapEndpoints();

app.UseHttpsRedirection();

await app.RunAsync();