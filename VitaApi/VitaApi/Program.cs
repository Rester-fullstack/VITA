using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using QuestPDF.Infrastructure;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Mappings;
using VitaApi.Middlewares;
using VitaApi.Repositories;
using VitaApi.Services;
using VitaApi.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder =
            System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });



builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    );
});



builder.Services.AddScoped<
AuthService>();

builder.Services.AddScoped<
    MedicoService>();

builder.Services.AddScoped<
    IPacienteService,
    PacienteService
>();

builder.Services.AddScoped<
    IConsultaService,
    ConsultaService
>();

builder.Services.AddScoped<
    IExameService,
    ExameService
>();

builder.Services.AddScoped<
    IEspecialidadeService,
    EspecialidadeService
>();

builder.Services.AddScoped<
    IHistoricoClinicoService,
    HistoricoClinicoService
>();

builder.Services.AddScoped<
    IReceitaService,
    ReceitaService
>();

builder.Services.AddScoped<
    IAtestadoService,
    AtestadoService
>();

builder.Services.AddScoped<
    IAtestadoRepository,
    AtestadoRepository
>();

builder.Services.AddScoped<
    INutricaoRepository,
    NutricaoRepository
>();

builder.Services.AddScoped<
    INutricaoService,
    NutricaoService
>();

builder.Services.AddScoped<
    ISolicitacaoExameRepository,
    SolicitacaoExameRepository
>();

builder.Services.AddScoped<
    ISolicitacaoExameService,
    SolicitacaoExameService
>();

builder.Services.AddScoped<
    IDeclaracaoComparecimentoRepository,
    DeclaracaoComparecimentoRepository
>();

builder.Services.AddScoped<
    IDeclaracaoComparecimentoService,
    DeclaracaoComparecimentoService
>();

builder.Services.AddScoped<
    DeclaracaoComparecimentoPdfService
>();

builder.Services.AddScoped<
    IProntuarioService,
    ProntuarioService
>();

builder.Services.AddScoped<
    ITimelineService,
    TimelineService
>();

builder.Services.AddScoped<
    IAuditoriaRepository,
    AuditoriaRepository
>();

builder.Services.AddScoped<
    IAuditoriaService,
    AuditoriaService
>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<
    IUserContextService,
    UserContextService
>();

builder.Services.AddScoped<
    INotificacaoService,
    NotificacaoService
>();

builder.Services.AddScoped<
    IConfiguracaoClinicaService,
    ConfiguracaoClinicaService
>();

builder.Services.AddScoped<
AtestadoPdfService>();

builder.Services.AddScoped<
ReceitaPdfService>();

builder.Services.AddScoped<
DashboardService>();

builder.Services.AddScoped<
IAgendaRepository,
AgendaRepository>();

builder.Services.AddScoped<
SolicitacaoExamePdfService>();

builder.Services.AddScoped<
    IPacienteRepository,
    PacienteRepository
>();

builder.Services.AddScoped<
    IConsultaRepository,
    ConsultaRepository
>();

builder.Services.AddScoped<
    IExameRepository,
    ExameRepository
>();

builder.Services.AddScoped<
    IHistoricoClinicoRepository,
    HistoricoClinicoRepository
>();

builder.Services.AddScoped<
    IMedicoRepository,
    MedicoRepository
>();

builder.Services.AddScoped<
    IEspecialidadeRepository,
    EspecialidadeRepository
>();

builder.Services.AddScoped<
    IUserRepository,
    UserRepository
>();

builder.Services.AddScoped<
    IRefreshTokenRepository,
    RefreshTokenRepository
>();

builder.Services.AddScoped<
    IReceitaRepository,
    ReceitaRepository
>();

builder.Services.AddScoped<
    IOdontogramaService,
    OdontogramaService
>();

builder.Services.AddScoped<
    IOdontogramaRepository,
    OdontogramaRepository
>();

builder.Services.AddScoped<
    IPsicologiaService,
    PsicologiaService
>();

builder.Services.AddScoped<
    IPsicologiaRepository,
    PsicologiaRepository
>();

builder.Services.AddScoped<
    IProntuarioRepository,
    ProntuarioRepository
>();

builder.Services.AddScoped<
    ProntuarioPdfService
>();



builder.Services
    .AddFluentValidationAutoValidation();

builder.Services
    .AddValidatorsFromAssemblyContaining<
        CreatePacienteValidator
    >();



builder.Services.AddAutoMapper(
    typeof(PacienteProfile).Assembly
);

builder.Services.AddAutoMapper
    (typeof(MedicoProfile).Assembly);



builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!
                    )
                ),

            ClockSkew = TimeSpan.Zero,

            RoleClaimType = System.Security.Claims.ClaimTypes.Role,
            NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
        };
    });



builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Vita API",
            Version = "v1",
            Description =
                "Sistema médico completo"
        }
    );

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Digite: Bearer {seu token}"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType
                                    .SecurityScheme,

                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        }
    );
});



builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    );
});

QuestPDF.Settings.License =
    LicenseType.Community;

var app = builder.Build();



app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionMiddleware>();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();