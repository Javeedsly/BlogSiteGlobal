using BlogSite.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSite.Business.Services.Abstract
{
    public interface IPortfolioService
    {
        Task AddPortfolio(Portfolio portfolio);
        void DeletePortfolio(int id);
        void UpdatePortfolio(int id, Portfolio newPortfolio);
        Portfolio GetPortfolio(Func<Portfolio, bool>? predicate = null);
        List<Portfolio> GetAllPortfolios(Func<Portfolio, bool>? predicate = null);
    }
}
