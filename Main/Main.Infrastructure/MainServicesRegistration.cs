using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Reflection;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Auth.Application.AuthServices;
using Main.Infrastructure.Authentications;
using Share.EmailService;
using Main.Infrastructure.Mails.PasswordResetMail;
using Microsoft.IdentityModel.Tokens;
using Main.Infrastructure.Database;

namespace Main.Infrastructure
{
    public static class MainServicesRegistration
    {
        public static IServiceCollection AddMainServices(this IServiceCollection services, Action<MainConfigData> options)
        {
            var authConf = new MainConfigData();
            options?.Invoke(authConf);
            services.AddDbContext<CrystalFreshDbContext>(options => options.UseNpgsql(authConf.MAIN_DB_CONNECT_STRING));

            services.AddSingleton(authConf);
            services.AddScoped<IAuthService, AuthService>();
            services.JWTService(authConf.AUTH_SECRET_KEY);
            services.AddAuthorizationPolicy();
            services.AddScoped<IMailGenerator<PasswordResetPayload>, PasswordResetMailGenerator>();
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisteredAssemblyHandlers(assembly);
            return services;
        }

        private static IServiceCollection JWTService(this IServiceCollection services, string secretKey)
        {
            var key = Encoding.UTF8.GetBytes(secretKey);
            services.AddAuthentication()
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


            return services;
        }
    }
}
