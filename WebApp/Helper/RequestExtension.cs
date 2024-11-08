using WebApp.DTOs;

namespace WebApp.Helper
{
    public static class RequestExtension
    {
        public static string FilterRequest(FilterDTO filter)
        {
            string filterRequset = string.Empty;
            
            if (filter.OrderBy != null)
                filterRequset += $"&OrderBy={filter.OrderBy}";
            if (filter.OrderByDescending != null)
                filterRequset += $"&OrderByDescending={filter.OrderByDescending}";

            return filterRequset;
        }
    }
}
