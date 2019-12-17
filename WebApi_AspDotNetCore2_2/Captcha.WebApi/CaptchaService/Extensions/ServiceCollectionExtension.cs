using Microsoft.Extensions.DependencyInjection;

namespace CaptchaService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddCaptchaService(this IServiceCollection services)
        {
            services.AddSingleton<ICaptchaFactory, CaptchaFactory>();
        }
    }
}
