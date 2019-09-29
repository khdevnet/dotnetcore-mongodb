using Books.Data.Domain.Extensibility.Repository.Write;
using Books.Data.UnitOfWork.Sql.Database;
using Books.Data.UnitOfWork.Sql.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Books.WebApi.Configurations
{
    public static class SqlDbContextConfigurationExtensions
    {
        public static void AddSqlDbContext(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<BooksSqlDbContext>(
                options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(BooksSqlDbContext).GetTypeInfo().Assembly.GetName().Name)));
        }

        public static void RegisterSqlServices(this IServiceCollection services)
        {
            services.AddTransient<IBookWriteRepository, BookWriteRepository>();
        }

        public static void ApplySqlDbMigrations(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                BooksSqlDbContext context = serviceScope.ServiceProvider.GetRequiredService<BooksSqlDbContext>();
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }
        }
    }
}
