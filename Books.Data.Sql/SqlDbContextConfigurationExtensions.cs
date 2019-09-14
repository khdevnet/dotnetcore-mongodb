using Books.Data.Sql.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Books.Data.Sql
{
    public static class SqlDbContextConfigurationExtensions
    {
        public static void AddSqlDbContext(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<BooksSqlDbContext>(
                options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(BooksSqlDbContext).GetTypeInfo().Assembly.GetName().Name)));
        }

        public static void ApplySqlDbMigrations(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                BooksSqlDbContext context = serviceScope.ServiceProvider.GetRequiredService<BooksSqlDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
