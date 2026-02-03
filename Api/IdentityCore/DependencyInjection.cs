using IdentityCore.AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.Business;
using IdentityCore.Repository.UnitOfWork;
using IdentityCore.Services.Interfaces;
using IdentityCore.Services;
using System.Reflection;

namespace IdentityCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, params string[] exclude)
        {
            var assembly = Assembly.Load("IdentityCore.Services");

            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service") && !exclude.Contains(t.Name))
                .ToList();

            foreach (var _implement in types)
            {
                var interfaces = _implement.GetInterfaces()
                    .Where(i => i.Name.EndsWith("Service"))
                    .ToList();

                foreach (var _interface in interfaces)
                {
                    services.AddScoped(_interface, _implement);
                }
            }

            return services;
        }

        public static IServiceCollection AddBusinesses(this IServiceCollection services, params string[] exclude)
        {
            var assembly = Assembly.Load("IdentityCore.Business");

            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Business") && !exclude.Contains(t.Name)) 
                .ToList();

            foreach (var _implement in types)
            {
                var interfaces = _implement.GetInterfaces()
                    .Where(i => i.Name.EndsWith("Business"))
                    .ToList();

                foreach (var _interface in interfaces)
                {
                    services.AddScoped(_interface, _implement);
                }
            }

            return services;
        }

        public static IServiceCollection AddRepositoties(this IServiceCollection services)
        {
            var assembly = Assembly.Load("IdentityCore.Repository");

            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .ToList();

            foreach (var _implement in types)
            {
                var interfaces = _implement.GetInterfaces()
                    .Where(i => i.Name.EndsWith("Repository"))
                    .ToList();

                foreach (var _interface in interfaces)
                {
                    services.AddScoped(_interface, _implement);
                }
            }

            return services;
        }

        public static IServiceCollection AddApplications(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ICronJobService, CronJobService>();
            services.AddTransient<ICronJobBusiness, CronJobBusiness>();

            services.AddHttpClient<IFCMService, FCMService>();


            return services;
        }
    }
}
