using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SF.Auth.DataAccess;
using SF.Auth.Repositories;
using SF.Auth.Repositories.Cache;
using SF.Auth.Repositories.Interfaces;
using SF.Auth.Services;
using SF.Auth.Services.Interfaces;
using SF.Common.DataAccess;
using SF.Common.DataAccess.Interface;
using SF.Common.Repositories;
using SF.Common.Repositories.Cache.Interfaces;
using SF.Common.Repositories.Interfaces;
using SF.Common.Settings.Database;
using SF.Common.Settings.Repositories;
using SF.Common.Settings.Repositories.Interfaces;

namespace SF.Auth.WebUI
{
    public class Startup
    {
        #region Public Properties

        public IConfiguration Configuration { get; }

        #endregion Public Properties

        #region Public Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods

        #region Private Methods

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

            services.AddSingleton<ICacheStorage, MemoryCacheAdpater>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHelpService, HelpService>();
        }

        #endregion Private Methods
    }
}