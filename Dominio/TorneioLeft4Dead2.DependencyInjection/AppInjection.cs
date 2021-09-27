using System;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.Steam.Context;
using TorneioLeft4Dead2.Storage.UnitOfWork;

namespace TorneioLeft4Dead2.DependencyInjection
{
    public static class AppInjection
    {
        public static void AddApp(this IServiceCollection services)
        {
            var assemblies = new[]
            {
                Assembly.Load("TorneioLeft4Dead2"),
                Assembly.Load("TorneioLeft4Dead2.Storage")
            };

            services.AddAutoMapper(assemblies);
            services.AddValidatorsFromAssemblies(assemblies);

            services.AddScoped(_ =>
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                var unitOfWorkStorage = new UnitOfWorkStorage(connectionString);

                return unitOfWorkStorage;
            });

            services.AddScoped<AuthContext>();

            services.Scan(scan => scan.FromAssemblies(assemblies).AddClasses().AsImplementedInterfaces());

            services.AddScoped(_ => SteamContext.PlayerService);
            services.AddScoped(_ => SteamContext.SteamUserService);
        }
    }
}