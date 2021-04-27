using Microsoft.Extensions.Configuration;

namespace ConversionTool.Configuration
{
    public static class ConfigurationExtensions
    {
        public static AppConfiguration GetApplicationConfig(this IConfiguration configuration)
        {
            var appConfig = new AppConfiguration();
            configuration.GetSection("AppConfiguration").Bind(appConfig);
            return appConfig;
        }
    }
}
