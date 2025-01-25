using System;

namespace UnecontLogConverter.Helpers
{
    public class Validations
    {
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static bool IsValidGuid(string input)
        {
            return Guid.TryParse(input, out _);
        }
    }
}
