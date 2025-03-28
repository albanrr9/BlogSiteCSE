namespace BlogSite.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
    }
}
