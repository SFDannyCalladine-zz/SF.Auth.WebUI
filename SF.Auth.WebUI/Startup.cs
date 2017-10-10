using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SF.Auth.DataAccess;
using SF.Auth.Repositories;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services;
using SF.Auth.Services.Interfaces;
using SF.Common.DataAccess;
using SF.Common.DataAccess.Interface;
using SF.Common.Repositories;
using SF.Common.Repositories.Interfaces;
using SF.Common.Settings.Database;
using SF.Common.Settings.Repositories;
using SF.Common.Settings.Repositories.Interfaces;
using System.Reflection;

namespace SF.Auth.WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            AddDbContexts(
                services,
                Configuration,
                migrationsAssembly);
            AddRepositories(services);
            AddServices(services);
            AddFactories(services);

            services.AddAutoMapper();
            services.AddMvc();

            AddIdentityServer(
                services,
                Configuration,
                migrationsAssembly);
        }

        private static void AddDbContexts(
            IServiceCollection services,
            IConfiguration configuration,
            string migrationsAssembly)
        {
            var adminConnectionString = configuration.GetConnectionString("StoreFeederRoot");
            var settingConnectionString = configuration.GetConnectionString("SFSetting");
            var helpConnectionString = configuration.GetConnectionString("SFHelp");

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<dbCustomerDatabase>();

            services.AddDbContext<dbRoot>(options =>
                options.UseSqlServer(adminConnectionString, builder =>
                    builder.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<dbSetting>(options =>
                options.UseSqlServer(settingConnectionString, builder =>
                    builder.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<dbHelp>(options =>
                options.UseSqlServer(helpConnectionString, builder =>
                    builder.MigrationsAssembly(migrationsAssembly)));
        }

        private static void AddFactories(IServiceCollection services)
        {
            services.AddScoped<IDbCustomerDatabaseFactory, DbCustomerDatabaseFactory>();
        }

        private static void AddIdentityServer(
            IServiceCollection services,
            IConfiguration configuration,
            string migrationsAssembly)
        {
            var identityConnectionString = configuration.GetConnectionString("SFIdentity");

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(identityConnectionString, ssOptions =>
                            ssOptions.MigrationsAssembly(migrationsAssembly)))
                .AddOperationalStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(identityConnectionString, ssOptions =>
                            ssOptions.MigrationsAssembly(migrationsAssembly)));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<IRootRepository, RootRepository>();
            services.AddSingleton<ISettingRepository, SettingRepository>();
            services.AddSingleton<IHelpRepository, HelpRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHelpService, HelpService>();
        }
    }
}