using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyOpinion.Domain;
using MyOpinion.Domain.Entities;
using MyOpinion.Service;
using MyOpinion.Services;

namespace MyOpinion.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticlesController : Controller
    {
        private readonly DataManager dataManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        public ArticlesController(DataManager dataManager, IWebHostEnvironment hostingEnvironment)
        {
            this.dataManager = dataManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Edit(Guid id)
        {
            var entity = id == default ? new Article() : dataManager.Articles.GetArticleById(id);
            return View(entity);
        }
        [HttpPost]
        public IActionResult Edit(Article model, IFormFile titleImageFile)
        {
            if (ModelState.IsValid)
            {
                Dictionary<string, string> dic = MySessionExtensions.Get<Dictionary<string, string>>(HttpContext.Session, KeysInTempData.Dictionary);
                if (titleImageFile != null)
                {
                    model.TitleImagePath = titleImageFile.FileName;
                    using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", titleImageFile.FileName), FileMode.Create))
                    {
                        titleImageFile.CopyTo(stream);
                    }
                }
                Guid.TryParse(dic[KeysInTempData.Id], out Guid authorId);
                model.AuthorsId = authorId;
                dataManager.Articles.SaveArticle(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            dataManager.Articles.DeleteArticle(id);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    
    }
}
