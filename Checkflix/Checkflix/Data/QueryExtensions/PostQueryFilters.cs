
namespace Checkflix.Data.QueryExtensions
{
    public class PostQueryFilters
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchQuery { get; set; }
        public bool IsNetflix { get; set; } = true;
        public bool IsHbo { get; set; } = true;
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public int? RatingFrom { get; set; }
        public int? RatingTo { get; set; }
        public int[] Categories { get; set; }
    }
}