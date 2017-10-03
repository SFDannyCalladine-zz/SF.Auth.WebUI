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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentityServer();

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

            AddIdentityServer(
                services,
                Configuration,
                migrationsAssembly);

            services.AddOptions();

            AddServices(services);
            AddFactories(services);
            AddRepositories(services);
            AddDbContexts(
                services,
                Configuration,
                migrationsAssembly);

            services.AddMvc();
            services.AddAutoMapper();
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

        private static void AddDbContexts(
            IServiceCollection services, 
            IConfiguration configuration,
            string migrationsAssembly)
        {
            var adminConnectionString = configuration.GetConnectionString("StoreFeederRoot");
            var settingConnectionString = configuration.GetConnectionString("SFSetting");

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<dbCustomerDatabase>();

            services.AddDbContext<dbAdmin>(options =>
                options.UseSqlServer(adminConnectionString, builder =>
                    builder.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<dbSetting>(options =>
                options.UseSqlServer(settingConnectionString, builder =>
                    builder.MigrationsAssembly(migrationsAssembly)));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<IAuthRepository, AuthRepository>();
            services.AddSingleton<ISettingRepository, SettingRepository>();
        }

        private static void AddFactories(IServiceCollection services)
        {
            services.AddScoped<IDbCustomerDatabaseFactory, DbCustomerDatabaseFactory>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}