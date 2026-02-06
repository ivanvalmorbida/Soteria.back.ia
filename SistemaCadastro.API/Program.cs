using SistemaCadastro.API.Configuration;
using SistemaCadastro.API.Repositories;
using SistemaCadastro.API.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var secret = jwtSettings.Get<JwtSettings>()?.Secret ?? throw new InvalidOperationException("JWT Secret not configured");
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Apenas para desenvolvimento
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Get<JwtSettings>()?.Issuer,
        ValidAudience = jwtSettings.Get<JwtSettings>()?.Audience,
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    // Política: Apenas Administrador
    options.AddPolicy("ApenasAdministrador", policy =>
        policy.RequireRole("1"));
    
    // Política: Usuário ou superior (Admin ou Usuário)
    options.AddPolicy("UsuarioOuSuperior", policy =>
        policy.RequireRole("1", "2"));
    
    // Política: Qualquer usuário autenticado
    options.AddPolicy("QualquerAutenticado", policy =>
        policy.RequireAuthenticatedUser());
});

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DatabaseConfig(connectionString!));

// Repositories
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IPessoaFisicaRepository, PessoaFisicaRepository>();
builder.Services.AddScoped<IPessoaJuridicaRepository, PessoaJuridicaRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
builder.Services.AddScoped<ICidadeRepository, CidadeRepository>();
builder.Services.AddScoped<IBairroRepository, BairroRepository>();
builder.Services.AddScoped<ICepRepository, CepRepository>();
builder.Services.AddScoped<IPessoaEmailRepository, PessoaEmailRepository>();
builder.Services.AddScoped<IPessoaEnderecoEletronicoRepository, PessoaEnderecoEletronicoRepository>();
builder.Services.AddScoped<IPessoaFoneRepository, PessoaFoneRepository>();
builder.Services.AddScoped<IPessoaTelefoneRepository, PessoaTelefoneRepository>();
builder.Services.AddScoped<IPessoaTelefoneRepository, PessoaTelefoneRepository>();
builder.Services.AddScoped<IPessoaTelefoneRepository, PessoaTelefoneRepository>();
builder.Services.AddScoped<ICBORepository, CBORepository>();
builder.Services.AddScoped<INacionalidadeRepository, NacionalidadeRepository>();
builder.Services.AddScoped<IAtividadeEconomicaRepository, AtividadeEconomicaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Services
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<IPessoaFisicaService, PessoaFisicaService>();
builder.Services.AddScoped<IPessoaJuridicaService, PessoaJuridicaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
