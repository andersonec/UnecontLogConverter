namespace UnecontLogConverter.Infrastructure
{
    public interface IAppSettingsService
    {
        string GetVersion();
        string GetProvider();
    }
}
