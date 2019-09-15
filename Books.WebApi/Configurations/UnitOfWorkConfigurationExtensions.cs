using Books.Data.UnitOfWork;
using Books.Data.UnitOfWork.Repository;
using Books.Domain.Extensibility.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.WebApi.Configurations
{
    public static class UnitOfWorkConfigurationExtensions
    {
        public static void RegisterUnitOfWorkServices(this IServiceCollection services)
        {
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<Func<IUnitOfWork>>((container) => () => container.GetService<IUnitOfWork>());
        }
    }
}
