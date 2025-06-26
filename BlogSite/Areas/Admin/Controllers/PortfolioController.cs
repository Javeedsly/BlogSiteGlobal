using BlogSite.Business.Exceptions;
using BlogSite.Business.Services.Abstract;
using BlogSite.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _portfolioService;


        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }
        public IActionResult Index()
        {
            var portfolios = _portfolioService.GetAllPortfolios();
            return View(portfolios);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Portfolio portfolio)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                await _portfolioService.AddPortfolio(portfolio);
            }
            catch (ImageContentException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (FileNullReferenceException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var existPortfolio = _portfolioService.GetPortfolio(x => x.Id == id);
            if (existPortfolio == null) return NotFound();
            return View(existPortfolio);
        }
        [HttpPost]
        public IActionResult Update(Portfolio portfolio)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                _portfolioService.UpdatePortfolio(portfolio.Id, portfolio);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound();
            }
            catch (ImageContentException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var existPortfolio = _portfolioService.GetPortfolio(x => x.Id == id);
            if (existPortfolio == null) return NotFound();
            return View(existPortfolio);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {

            try
            {
                _portfolioService.DeletePortfolio(id);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
