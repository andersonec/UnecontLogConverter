namespace UnecontLogConverter.ViewModels
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string Provider { get; set; } = "MINHA CDN";
        public string HttpMethod { get; set; }
        public int StatusCode { get; set; }
        public string UriPath { get; set; }
        public int TimeTaken { get; set; }
        public int ResponseSize { get; set; }
        public string CacheStatus { get; set; }
    }
}