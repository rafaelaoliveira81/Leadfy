using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository.Repositories;
using Api.Shared.Middleware;
using Repository.Context;
using Domain.Interfaces;
using System.Reflection;
using Domain.Config;
using Application;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
                  ?? throw new InvalidOperationException("JwtSettings não foi configurado.");
var allowedOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>()
                     ?? new[] { "http://localhost:3000" };

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Configura o Swagger para incluir comentários XML
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Informe o token JWT no formato: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Adicione serviços ao contêiner.
builder.Services.AddScoped<IUserApp, UserApp>();
builder.Services.AddScoped<IAuthenticationApp, AuthenticationApp>();
builder.Services.AddScoped<IJwtApp, JwtApp>();
builder.Services.AddScoped<IPasswordRecoveryApp, PasswordRecoveryApp>();
builder.Services.AddScoped<IEmailApp, EmailService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IOwnerApp, OwnerApp>();
builder.Services.AddScoped<ILeadApp, LeadApp>();
builder.Services.AddScoped<IProductApp, ProductApp>();
builder.Services.AddScoped<IOpportunityApp, OpportunityApp>();
builder.Services.AddScoped<IOpportunityActionPlanApp, OpportunityActionPlanApp>();
builder.Services.AddScoped<IInteractionApp, InteractionApp>();
builder.Services.AddScoped<IAiConfigApp, AiConfigApp>();
builder.Services.AddScoped<IApiKeyEncryptionService, ApiKeyEncryptionService>();
builder.Services.AddScoped<IAiService, AiService>();
builder.Services.AddScoped<IGenerativeAiService, AiService>();

// Adicione as interfaces de banco de dados
builder.Services.AddScoped<IUserRepo, UserRepository>();
builder.Services.AddScoped<IOwnerRepo, OwnerRepo>();
builder.Services.AddScoped<IPasswordRecoveryRepo, PasswordRecoveryRepo>();
builder.Services.AddScoped<ILeadRepo, LeadRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IOpportunityRepo, OpportunityRepo>();
builder.Services.AddScoped<IInteractionRepo, InteractionRepo>();
builder.Services.AddScoped<IAiConfigRepo, AiConfigRepo>();

// Adiciona os serviços
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection não foi configurada. Defina em appsettings.json, appsettings.{Environment}.json ou variável de ambiente.");
}

// Adicione o serviço de banco de dados
builder.Services.AddDbContext<CRMContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();