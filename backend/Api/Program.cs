using Application;
using Domain.Config;
using Repository.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configura o Swagger para incluir comentários XML
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Adicione serviços ao contêiner.
builder.Services.AddScoped<IUserApp, UserApp>();
builder.Services.AddScoped<IAuthenticationApp, AuthenticationApp>();
builder.Services.AddScoped<IJwtApp, JwtApp>();
builder.Services.AddScoped<IUserGroupPermissionApp, UserGroupPermissionApp>();
builder.Services.AddScoped<IPermissionApp, PermissionApp>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IOwerApp, OwerApp>();
builder.Services.AddScoped<ILeadApp, LeadApp>();
builder.Services.AddScoped<IProductApp, ProductApp>();
builder.Services.AddScoped<IOpportunityApp, OpportunityApp>();
builder.Services.AddScoped<IInteractionApp, InteractionApp>();
builder.Services.AddScoped<IAiConfigApp, AiConfigApp>();
builder.Services.AddScoped<IApiKeyEncryptionService, ApiKeyEncryptionService>();
builder.Services.AddScoped<IAiService, AiService>();

// Adicione as interfaces de banco de dados
builder.Services.AddScoped<IUserRepo, UserRepository>();
builder.Services.AddScoped<IUserGroupRepo, UserGroupRepo>();
builder.Services.AddScoped<IPermissionRepo, PermissionRepo>();
builder.Services.AddScoped<IUserGroupPermissionRepo, UserGroupPermissionRepo>();
builder.Services.AddScoped<IPasswordRecoveryRepo, PasswordRecoveryRepo>();
builder.Services.AddScoped<IOwerRepo, OwerRepo>();
builder.Services.AddScoped<ILeadRepo, LeadRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IOpportunityRepo, OpportunityRepo>();
builder.Services.AddScoped<IInteractionRepo, InteractionRepo>();
builder.Services.AddScoped<IAiConfigRepo, AiConfigRepo>();

// Adiciona os serviços
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .SetIsOriginAllowedToAllowWildcardSubdomains()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Adicione o serviço de banco de dados
builder.Services.AddDbContext<CRMContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();