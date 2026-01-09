using Hangfire;
using IdentityCore;
using IdentityCore.AutoMapper;
using IdentityCore.Business;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.Filters;
using IdentityCore.Middlewares;
using IdentityCore.Repository;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;
using IdentityCore.Services;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;
using static IdentityCore.HangfireService;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, o =>
    {
        o.Protocols = HttpProtocols.Http1;
        o.UseHttps();
    });

    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps();
    });
});


var build = new ConfigurationBuilder().AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false);
IConfiguration configuration = build.Build();

SystemConfig.BuildSystemConfig(configuration);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<Configuration>(configuration);
builder.Services.AddDbContext<IdentityContext>(options
             => options.UseSqlServer(config.GetConnectionString("DbContext"),b => b.MigrationsAssembly("IdentityCore.EFs")));


builder.Services.AddCors(options => options.AddPolicy("IdentityCore_Policy", p => p
                                                                   .WithOrigins("http://localhost:5173")
                                                                   .AllowAnyMethod()
                                                                   .AllowAnyHeader()));

builder.Services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(config.GetConnectionString("HangsfireContenxt")));

builder.Services.AddHangfireServer()
        .AddHangfire(config, config.GetConnectionString("HangsfireContenxt")!);

builder.Services.AddLogging();
builder.Services.AddTransient<GlobalHandlerMiddleware>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
    options.Filters.Add(typeof(GlobalEndPointFilter));
}).AddNewtonsoftJson(x =>
{
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

builder.Services.AddStackExchangeRedisCache(action => {
    var connection = $"{GlobalConst.Redis.Url}:{GlobalConst.Redis.Port}"; 
    action.Configuration = connection;
});

#region AutoMapper Profile
builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion

#region CronJobs
builder.Services.AddTransient<ICronJobService, CronJobService>();
builder.Services.AddTransient<ICronJobBusiness, CronJobBusiness>();
#endregion

#region Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService,RoleService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IFileService, FileService>();
#endregion

#region Business
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<IRoleBusiness, RoleBusiness>();
builder.Services.AddScoped<IPermissionBusiness, PermissionBusiness>();
builder.Services.AddScoped<IEmailBusiness, EmailBusiness>();
builder.Services.AddScoped<IAuthenticationBusiness, AuthenticationBusiness>();
builder.Services.AddScoped<IServiceBusiness, ServiceBusiness>();
#endregion

#region Repo
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IUserServiceRepository, UserServiceRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
#endregion

#region UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<GlobalResponseOperationFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Identity API",
        Description = "An ASP.NET Core Web API for managing identity items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://example.com/license")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{ }
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
#endregion


builder.Services.AddGrpc();


var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<IdentityContext>();
var logger = services.GetService<ILogger<LoggerExtension>>();
var cronJob = services.GetService<ICronJobService>();

if(builder.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(!string.IsNullOrWhiteSpace(HangfireConfig.DashboardUrl)
        ? $"Hangfire Access Dashboard via Url: {HangfireConfig.DashboardUrl}?{HangfireConfig.AccessKeyQueryParam}={HangfireConfig.AccessKey}"
        : "Hangfire Setup without Dashboard");
    Console.ResetColor();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<GlobalHandlerMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<HangfireDashboardAccessMiddleware>();


app.UseHangfireDashboard(HangfireConfig.DashboardUrl, new DashboardOptions
{
    Authorization = new[] { new CustomAuthorizeFilter() },
    AppPath = HangfireConfig.BackToSiteUrl,
    StatsPollingInterval = HangfireConfig.StatsPollingInterval
});
app.UseRouting();
app.UseCors("IdentityCore_Policy");

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.MapGrpcService<IdentityGrpcService>();
app.MapHangfireDashboard();

cronJob!.RunCronJobs();

try
{
    await context.Database.MigrateAsync();
    await IdentityContextSeed.SeedAsync(context);
}
catch(Exception ex)
{
    logger!.LogError(ex,ex.Message);
}


app.Run();

