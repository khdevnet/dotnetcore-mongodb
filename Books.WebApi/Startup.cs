using Books.Data.FileStorage;
using Books.Data.FileStorage.Provider;
using Books.Data.NoSql.Database;
using Books.Data.Sql;
using Books.Data.Sql.Database;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Provider;
using Books.WebApi.Configurations;
using Books.WebApi.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

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

            services.RegisterDomainServices();

            services.AddSingleton<IFileStoragePathProvider, FileStoragePathProvider>();
            services.AddSingleton<IBookFilePathProvider, BookFilePathProvider>();
            services.AddSingleton<IBookFileStorage, BookFileStorage>();
            services.AddSingleton<IModelConverter, ModelConverter>();

            services.AddMvc()
                    .AddJsonOptions(options => options.UseMemberCasing())
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Books Web API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Anton Shchcherbyna", Email = "", Url = "" },
                    License = new License { Name = "Apache 2.0", Url = "http://www.apache.org/licenses/LICENSE-2.0" }
                });

                c.OperationFilter<FormFileSwaggerFilter>();

                //Determine base path for the application.
                var basePath = AppContext.BaseDirectory;

                //Set the comments path for the swagger json and ui.
                // c.IncludeXmlComments(Path.Combine(basePath + "/Books.WebApi.xml"));
            });
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseMvc();
        }
    }
}
