using Books.Core;
using Books.Data.UnitOfWork;
using Books.Domain.Extensibility.Service;
using Books.Domain.Repository;
using Books.Domain.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.WebApi.Configurations
{
    public static class DomainConfigurationExtensions
    {
        public static void RegisterDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IBookService, BookService>();
        }
    }
}
