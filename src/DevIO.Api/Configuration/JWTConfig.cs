using DevIO.Api.ObjectValues;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    public static class JWTConfig
    {
        public static IServiceCollection AddJWRConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSettingsSection = configuration.GetSection("TokenSettings");
            services.Configure<TokenSettings>(tokenSettingsSection);

            var tokenSettings = tokenSettingsSection.Get<TokenSettings>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.ChaveSecreta)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = tokenSettings.ValidoEm,
                    ValidIssuer = tokenSettings.Emissor
                };
            });

            return services;
        }
    }
}
