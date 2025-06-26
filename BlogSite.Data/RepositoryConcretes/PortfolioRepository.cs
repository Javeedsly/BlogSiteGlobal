using BlogSite.Core.Models;
using BlogSite.Core.RepositoryAbstract;
using BlogSite.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSite.Data.RepositoryConcretes
{
    public class PortfolioRepository : GenericRepository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
