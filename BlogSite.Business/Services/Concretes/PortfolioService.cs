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
        // 1) Fayl yoxdursa exception
        if (portfolio.ImageFile == null || portfolio.ImageFile.Length == 0)
            throw new System.IO.FileNotFoundException("Fayl boş ola bilməz!");

        // 2) FileHelper ilə serverə yaz və browser üçün URL al
        //    "uploads/portfolios" qovluğunu wwwroot altında gözləyir
        portfolio.ImagePath = Helper.SaveImage(
            _env,
            portfolio.ImageFile,
            "uploads/portfolios"
        );

        // 3) Repository-dən əlavə et
        await _portfolioRepository.AddAsync(portfolio);
        await _portfolioRepository.CommitAsync();
    }

    public void DeletePortfolio(int id)
    {
        var existPortfolio = _portfolioRepository.Get(x => x.Id == id);
        if (existPortfolio == null)
            throw new EntityNotFoundException("Portfolio tapılmadı!");

        // Faylı sil
        // existPortfolio.ImagePath == "/uploads/portfolios/abcd.jpg"
        Helper.DeleteImage(_env, existPortfolio.ImagePath);

        // Database-dən sil
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
        // 1) Mövcud portfolionu götür
        var oldPortfolio = _portfolioRepository.Get(x => x.Id == id);
        if (oldPortfolio == null)
            throw new EntityNotFoundException("Portfolio tapılmadı!");

        // 2) Əgər yeni fayl varsa, köhnəni sil və yenisini yaz
        if (newPortfolio.ImageFile != null && newPortfolio.ImageFile.Length > 0)
        {
            // 2.1) Köhnə şəkli sil
            if (!string.IsNullOrWhiteSpace(oldPortfolio.ImagePath))
            {
                Helper.DeleteImage(_env, oldPortfolio.ImagePath);
            }

            // 2.2) Yeni şəkli saxla və relative URL-i al
            oldPortfolio.ImagePath = Helper.SaveImage(
                _env,
                newPortfolio.ImageFile,
                "uploads/portfolios"
            );
        }

        // 3) Digər sahələri yenilə
        oldPortfolio.Title = newPortfolio.Title;
        oldPortfolio.ProjectUrl = newPortfolio.ProjectUrl;

        // 4) Dəyişiklikləri DB-ə yaz
        _portfolioRepository.Commit();
    }
}
