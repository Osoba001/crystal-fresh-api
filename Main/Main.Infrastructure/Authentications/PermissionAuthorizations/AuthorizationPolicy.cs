using Main.Application.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Infrastructure.Authentications.PermissionAuthorizations
{
    public static class AuthorizationPolicy
    {
        public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(Permission.Administrator.ToString(), policy => policy.RequireRole(Role.Admin.ToString(),
                    Role.SuperAdmin.ToString()))
                .AddPolicy(Permission.SuperAdministrator.ToString(), policy => policy.RequireRole(Role.SuperAdmin.ToString()))
                .AddPolicy(Permission.User.ToString(), policy => policy.RequireRole(Role.User.ToString(),
                  Role.SuperAdmin.ToString()));
            return services;
        }
    }
}
