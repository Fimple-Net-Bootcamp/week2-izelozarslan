namespace space_weather_api;


    public class FilteringParameters
    {
        public DateTime? Date { get; set; }

    }

    public class PagingParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SortingParameters
    {
        public string? OrderBy { get; set; }
        public string? SortOrder { get; set; } //asc-desc
    }
