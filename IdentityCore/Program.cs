using AutoMapper;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


var build = new ConfigurationBuilder().AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false);
IConfiguration configuration = build.Build();

SystemConfig.BuildSystemConfig(configuration);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<Configuration>(configuration);
builder.Services.AddDbContext<IdentityContext>(options
             => options.UseSqlServer(config.GetConnectionString("DbContext"),b => b.MigrationsAssembly("IdentityCore.EFs")));


builder.Services.AddCors(options => options.AddPolicy("IdentityCore_Policy", p => p
                                                                   .AllowAnyOrigin()
                                                                   .AllowAnyMethod()
                                                                   .AllowAnyHeader()));
builder.Services.AddLogging();
builder.Services.AddTransient<GlobalHandlerMiddleware>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalEndPointFilter));
})
    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddStackExchangeRedisCache(action => {
    var connection = "localhost:6379"; 
    action.Configuration = connection;
});

#region AutoMapper Profile
builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion

#region Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService,RoleService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
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
builder.Services.AddScoped<IUserRolePermissionRepository, UserRolePermissionRepository>();
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
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


var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<IdentityContext>();
var logger = services.GetService<ILogger<LoggerExtension>>();

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
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<GlobalHandlerMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthentication();
app.UseCors("IdentityCore_Policy");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

try
{
    await context.Database.MigrateAsync();
    await IdentityContextSeed.SeedAsync(context);
}
catch(Exception ex)
{
    logger.LogError(ex,ex.Message);
}


app.Run();

