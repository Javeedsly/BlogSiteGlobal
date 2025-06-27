using BlogSite.Business.Exceptions;
using BlogSite.Business.Services.Abstract;
using BlogSite.Core.Models;
using BlogSite.Core.RepositoryAbstract;
using Microsoft.AspNetCore.Hosting;

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
            throw new System.IO.FileNotFoundException("Fayl boş ola bilməz!");

        portfolio.ImagePath = Helper.SaveFile(_env.WebRootPath, "uploads/portfolios", portfolio.ImageFile);
        await _portfolioRepository.AddAsync(portfolio);
        await _portfolioRepository.CommitAsync();
    }

    public void DeletePortfolio(int id)
    {
        var existPortfolio = _portfolioRepository.Get(x => x.Id == id);
        if (existPortfolio == null)
            throw new EntityNotFoundException("Portfolio tapılmadı!");

        // Fayl adını çıxarmaq
        string fileName = Path.GetFileName(existPortfolio.ImagePath);
        Helper.DeleteFile(_env.WebRootPath, "uploads/portfolios", fileName);

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
            throw new EntityNotFoundException("Portfolio tapılmadı!");

        if (newPortfolio.ImageFile != null)
        {
            // Köhnə fayl varsa sil
            if (!string.IsNullOrEmpty(oldPortfolio.ImagePath))
            {
                string oldFileName = Path.GetFileName(oldPortfolio.ImagePath);
                Helper.DeleteFile(_env.WebRootPath, "uploads/portfolios", oldFileName);
            }

            // Yeni faylı əlavə et
            oldPortfolio.ImagePath = Helper.SaveFile(_env.WebRootPath, "uploads/portfolios", newPortfolio.ImageFile);
        }

        oldPortfolio.Title = newPortfolio.Title;
        oldPortfolio.ProjectUrl = newPortfolio.ProjectUrl;

        _portfolioRepository.Commit();
    }
}
