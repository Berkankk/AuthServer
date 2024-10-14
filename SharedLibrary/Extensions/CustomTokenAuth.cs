using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Text;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth //Burayı tüm apilerde geçtiğimiz için ortak kısma yazdık bunlar token için gerekli

    {
        public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOptions customTokenOptions)
        {
            // Authentication servisini ekliyoruz
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                opts.RequireHttpsMetadata = false; // Geliştirme ortamında HTTPS gerekmiyor
                opts.SaveToken = true; // Token bellekte saklansın mı?

                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Issuer kontrolü yapılacak mı?
                    ValidateAudience = true, // Audience kontrolü yapılacak mı?
                    ValidateLifetime = true, // Token süresi kontrol edilecek mi?
                    ValidateIssuerSigningKey = true, // İmza kontrolü yapılacak mı?

                    ValidIssuer = customTokenOptions.Issuer, // Beklenen issuer
                    ValidAudiences = customTokenOptions.Audience, // Beklenen audience listesi
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(customTokenOptions.SecurityKey), // İmza anahtarı

                    ClockSkew = TimeSpan.Zero // Süre sapması olmaması için 0 verdik
                };
            });
        }
    }
}
