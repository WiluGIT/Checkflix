
namespace Checkflix.ViewModels
{
    public class UserCollectionFilter
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool? Favourites { get; set; }
        public bool? ToWatch { get; set; }
        public bool? Watched { get; set; }
        public string UserId { get; set; }
    }
}