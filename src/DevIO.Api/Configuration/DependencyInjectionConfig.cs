using DevIO.Api.ObjectValues;
using DevIO.Business.Interfaces;
using DevIO.Business.Services;
using DevIO.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using static DevIO.Api.Configuration.SwaggerConfig;

namespace DevIO.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddScoped<INotificadorService, NotificadorService>();

            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();

            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUsuarioIdentity, UsuarioIdentity>();

            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}
