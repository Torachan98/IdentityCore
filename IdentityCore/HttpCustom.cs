namespace IdentityCore
{
    public static class HttpCustom
    {
        public static bool IsRequestFor(this HttpRequest request, string endpoint)
        {
            // get path query with out query param string
            var pathQuery = request.Path.Value?.Trim('/').ToLower() ?? String.Empty;
            var iPathQueryWithoutParam = pathQuery.IndexOf('?');
            pathQuery = iPathQueryWithoutParam > 0 ? pathQuery.Substring(iPathQueryWithoutParam) : pathQuery;
            pathQuery = pathQuery.ToLowerInvariant();

            // get endpoint without query param string
            endpoint = endpoint.Trim('/');
            var iEndpointWithoutParam = endpoint.IndexOf('?');
            endpoint = iEndpointWithoutParam > 0 ? endpoint.Substring(0, iEndpointWithoutParam) : endpoint;
            endpoint = endpoint.ToLowerInvariant();

            // check quest is swagger endpoint
            var isRequestTheEndpoint = pathQuery == endpoint;
            return isRequestTheEndpoint;
        }
    }
}
