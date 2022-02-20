using DevIO.Business.Interfaces;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DevIODbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"));
            });

            services.AddScoped<INotificadorService, NotificadorService>();

            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();

            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        }
    }
}
