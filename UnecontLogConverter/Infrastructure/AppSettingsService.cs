using Microsoft.Extensions.Options;

namespace UnecontLogConverter.Infrastructure
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly AppSettings _appSettings;

        public AppSettingsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GetVersion()
        {
            return _appSettings.Version;
        }

        public string GetProvider()
        {
            return _appSettings.Provider;
        }
    }
}
