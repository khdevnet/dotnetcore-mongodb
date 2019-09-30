using Books.Data.UnitOfWork.NoSql.Database;
using Books.Data.UnitOfWork.NoSql.Repository;
using Books.Domain.Extensibility.Repository.Write;
using Books.Domain.Read.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Books.WebApi.Configurations
{
    public static class NoSqlDbContextConfigurationExtensions
    {
        public static void AddNoSqlDbContext(this IServiceCollection services, IConfigurationSection settings)
        {
            services.Configure<BooksNoSqlDbContextSettings>(settings);

            services.AddSingleton<IBookNoSqlDbContextSettings>(sp =>
                sp.GetRequiredService<IOptions<BooksNoSqlDbContextSettings>>().Value);

        }

        public static void RegisterNoSqlServices(this IServiceCollection services)
        {
            services.AddScoped<BooksNoSqlDbContext>();
            services.AddTransient<IBookReadRepository, BookReadRepository>();
        }

        public static void ApplyNoSqlDbMigrations(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var bookWriteRepository = serviceScope.ServiceProvider.GetRequiredService<IBookWriteRepository>();
                var bookReadRepository = serviceScope.ServiceProvider.GetRequiredService<IBookReadRepository>();

                bookReadRepository.Clear();
                var books = bookWriteRepository.Get();
                bookReadRepository.CreateBulk(books);

            }
        }
    }
}
