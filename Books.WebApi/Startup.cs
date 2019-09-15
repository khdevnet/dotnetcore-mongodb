using Books.Data.NoSql.Database;
using Books.Data.Sql;
using Books.Data.Sql.Database;
using Books.Domain.Extensibility.Provider;
using Books.WebApi.Configurations;
using Books.WebApi.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Books.WebApi
{
    public class Startup
    {
        private const string CorsPolicyName = "CorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    CorsPolicyName,
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSqlDbContext(Configuration.GetConnectionString(nameof(BooksSqlDbContext)));
            services.RegisterSqlServices();

            services.AddNoSqlDbContext(Configuration.GetSection(nameof(BooksNoSqlDbContextSettings)));
            services.RegisterNoSqlServices();

            services.RegisterUnitOfWorkServices();

            services.AddSingleton<IFileStoragePathProvider, FileStoragePathProvider>();
            services.AddSingleton<IBookFilePathProvider, BookFilePathProvider>();

            services.AddMvc()
                    .AddJsonOptions(options => options.UseMemberCasing())
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ApplySqlDbMigrations();
            app.ApplyNoSqlDbMigrations();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
