using BlogSite.Core.Models;

namespace BlogSite.Areas.Admin.ViewModels
{
    public class DashboardVM
    {
        public IEnumerable<ContactMessage> ContactMessages { get; set; }
        public List<Portfolio> Portfolios { get; set; }
    }

}
