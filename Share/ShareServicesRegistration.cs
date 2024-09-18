using Microsoft.Extensions.DependencyInjection;
using Share.EmailService;
using Share.FileManagement;
using Share.MediatKO;
using Swinva.share;

namespace Share
{
    public static class ShareServicesRegistration
    {

        public static IServiceCollection AddShareServices(this IServiceCollection services, Action<ShareConfigData> options)
        {
            var config = new ShareConfigData();
            options?.Invoke(config);

            services.AddSingleton(config.EMAIL_CONFIGURATION);
            services.AddSingleton(config.DEPLOYMENT_CONFIGURATION);
            services.AddSingleton(config.ObjectStorageConfiguration);
            services.AddScoped<IFileManagementService, ObjectStorageService>();
            services.AddScoped<IMailSender, EmailKitService>();
            services.AddScoped<IMediator, Mediator>();
            return services;
        }
    }
}
