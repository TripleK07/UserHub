using System;
namespace UserHubAPI.Helper
{
    public class Utility
    {
        public static String GetCurrentApiUrl(HttpRequest httpRequest)
        {
            // Get the current API URL
            return $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
        }
    }
}

