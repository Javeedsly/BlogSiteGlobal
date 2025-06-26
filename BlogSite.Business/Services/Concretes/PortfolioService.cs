using BlogSite.Business.Exceptions;
using BlogSite.Business.Extensions;
using BlogSite.Business.Services.Abstract;
using BlogSite.Core.Models;
using BlogSite.Core.RepositoryAbstract;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileNotFoundException = BlogSite.Business.Exceptions.FileNotFoundException;

namespace BlogSite.Business.Services.Concretes
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IWebHostEnvironment _env;

        public PortfolioService(IPortfolioRepository portfolioRepository, IWebHostEnvironment env)
        {
            _portfolioRepository = portfolioRepository;
            _env = env;
        }

        public async Task AddPortfolio(Portfolio portfolio)
        {
            if (portfolio.ImageFile == null)
                throw new FileNotFoundException("File bos ola bilmez!");

            portfolio.ImageUrl = Helper.SaveFile(_env.WebRootPath, @"uploads/portfolios", portfolio.ImageFile);
            await _portfolioRepository.AddAsync(portfolio);
            await _portfolioRepository.CommitAsync();
        }

        public void DeletePortfolio(int id)
        {
            var existPortfolio = _portfolioRepository.Get(x => x.Id == id);
            if (existPortfolio == null)
                throw new EntityNotFoundException("Portfolio tapilmadi!");
            Helper.DeleteFile(_env.WebRootPath, @"uploads\portfolios", existPortfolio.ImageUrl);

            _portfolioRepository.Delete(existPortfolio);
            _portfolioRepository.Commit();
        }

        public List<Portfolio> GetAllPortfolios(Func<Portfolio, bool>? predicate = null)
        {
            return _portfolioRepository.GetAll(predicate);
        }

        public Portfolio GetPortfolio(Func<Portfolio, bool>? predicate = null)
        {
            return _portfolioRepository.Get(predicate);
        }

        public void UpdatePortfolio(int id, Portfolio newPortfolio)
        {
            var oldPortfolio = _portfolioRepository.Get(x => x.Id == id);

            if (oldPortfolio == null)
                throw new EntityNotFoundException("Portfolio tapilmadi!");

            if (newPortfolio.ImageFile != null)
            {
                Helper.DeleteFile(_env.WebRootPath, @"uploads\portfolios", oldPortfolio.ImageUrl);
                oldPortfolio.ImageUrl = Helper.SaveFile(_env.WebRootPath, @"uploads\portfolios", newPortfolio.ImageFile);
            }
            oldPortfolio.Title = newPortfolio.Title;
            oldPortfolio.ProjectUrl = newPortfolio.ProjectUrl;



            _portfolioRepository.Commit();
        }
    }
}
