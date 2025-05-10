using System.Collections.Specialized;

namespace Psotify.HelpClasses
{
    public static class Paginator
    {
        public static (string prev, string next) GeneatePageURls(HttpRequest request,int pageNumber, int totalPages, int pageSize)
        {
            var baseUrl = $"{request.Scheme}://{request.Host}{request.Path}";
            var nextPageUrl = pageNumber < totalPages
                ? $"{baseUrl}?pageNumber={pageNumber + 1}&pageSize={pageSize}"
                : null;

            var previousPageUrl = pageNumber > 1
                ? $"{baseUrl}?pageNumber={pageNumber - 1}&pageSize={pageSize}"
                : null;
            return (previousPageUrl, nextPageUrl);
        }

        public static void AddPaginationToHeader(string nextPageUrl, string previousPageUrl, HttpResponse response)
        {
            if (!string.IsNullOrEmpty(nextPageUrl))
                response.Headers.Append("X-Next-Page", nextPageUrl);

            if (!string.IsNullOrEmpty(previousPageUrl))
                response.Headers.Append("X-Previous-Page", previousPageUrl);
        }
    }
}
