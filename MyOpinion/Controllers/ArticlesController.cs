using CKSource.CKFinder.Connector.Core.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MyOpinion.Domain;
using MyOpinion.Domain.Entities;
using MyOpinion.Models;
using MyOpinion.Service;
using MyOpinion.Services;

namespace MyOpinion.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly DataManager dataManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        public ArticlesController(DataManager dataManager, IWebHostEnvironment hostingEnvironment)
        {
            this.dataManager = dataManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        //public IActionResult AllArticles()
        //{
         
        //    ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("Articles");
        //    return View(dataManager.Articles.GetArticles());
        //}

        public IActionResult Show(Guid id) 
        {
            var artToShow = dataManager.Articles.GetArticleById(id);
            var artAuthore = dataManager.Users.GetUserById(artToShow.AuthorsId.ToString());
            MySessionExtensions.Set<IdentityUser>(HttpContext.Session, KeysInTempData.User, artAuthore);
            return View(artToShow);
        }

        public IActionResult UsersArticles() 
        {
            ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("Articles");
            var dic = MySessionExtensions.Get<Dictionary<string, string>>(HttpContext.Session, KeysInTempData.Dictionary);
            Guid.TryParse(dic[KeysInTempData.Id], out Guid id);
            var model = new ArticlesModel();
            model.articles = dataManager.Articles.GetArticlesByAuthorsId(id);
            return View("UsersArticles", model);
        }
        [HttpPost]
        public IActionResult UsersArticles(ArticlesModel model)
        {
            ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("Articles");
            var dic = MySessionExtensions.Get<Dictionary<string, string>>(HttpContext.Session, KeysInTempData.Dictionary);
            Guid.TryParse(dic[KeysInTempData.Id], out Guid id);
            if (model.text == null || model.text.Trim() == "" || model.Subject == null)
            {               
                model.articles = dataManager.Articles.GetArticlesByAuthorsId(id);

                return View("UsersArticles", model);
            }

            model.articles = Search(dataManager.Articles.GetArticlesByAuthorsId(id), model.text, model.Subject);
            return View("UsersArticles", model);
        }

        public IActionResult ShowAuthorsArticles()
        {
            ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("Articles");
            var user = MySessionExtensions.Get<IdentityUser>(HttpContext.Session, KeysInTempData.User);
            Guid.TryParse(user.Id, out Guid id);          
            var model = new ArticlesModel();
            model.articles = dataManager.Articles.GetArticlesByAuthorsId(id);
            return View("ShowAuthorsArticles",model);
        }
        [HttpPost]
        public IActionResult ShowAuthorsArticles(ArticlesModel model)
        {
            ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("Articles");
            var Id = MySessionExtensions.Get<IdentityUser>(HttpContext.Session, KeysInTempData.User).Id;
            Guid.TryParse(Id, out Guid id);
            if (model.text == null || model.text.Trim() == "" || model.Subject == null)
            {               
                model.articles = dataManager.Articles.GetArticlesByAuthorsId(id);

                return View("ShowAuthorsArticles", model);
            }

            model.articles = Search(dataManager.Articles.GetArticlesByAuthorsId(id), model.text, model.Subject);
            return View("ShowAuthorsArticles", model);
        }

        public IActionResult Edit(Guid id)
        {
            if(id == default) { return View(new Article()); }
            var article = dataManager.Articles.GetArticleById(id);
            if (article.AdditionalTitleImagesPath != null)
            {
                var array = article.AdditionalTitleImagesPath.Split(" ");
                for (var i = 0; i < 10; i++)
                {
                    if (array[i] != null && array[i] != "")
                    {
                        article.AdditImagPathes[i] = array[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return View(article);
            
        }
        [HttpPost]
        public IActionResult Edit(Article model, IFormFile ?titleImageFile, IFormFile[]? additImagPathes, string? ImgToDel)
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
                if(ImgToDel != null)
                {
                    var article = DeleteAddImg(ImgToDel, model);
                    return View("Edit", article);
                }
                if(additImagPathes != null)
                {
                    string allfilesstring = null;
                    foreach(var file in additImagPathes)
                    {
                        if (file != null)
                        {
                            allfilesstring += file.FileName + ' ';
                            using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", file.FileName), FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                    
                    model.AdditionalTitleImagesPath += allfilesstring;
                    
                }
                Guid.TryParse(dic[KeysInTempData.Id], out Guid authorId);
                if (!User.IsInRole("admin"))
                {
                    model.AuthorsId = authorId;
                }
                dataManager.Articles.SaveArticle(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(Guid Id)
        {
            dataManager.Articles.DeleteArticle(Id);
            MySessionExtensions.Set<Article>(HttpContext.Session, KeysInTempData.Article, null);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
        
        public Article DeleteAddImg(string file, Article article)
        {
           
            var paths = article.AdditionalTitleImagesPath.Split(" ").ToList();
            foreach(var path in paths)
            {
                if (path == file)
                {
                    paths.Remove(path);
                    using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", file), FileMode.Open))
                    {
                        stream.SetLength(0);
                    }
                    break;
                }

            }
            article.AdditionalTitleImagesPath = null;
            foreach (var path in paths)
            {
                article.AdditionalTitleImagesPath += path + ' ';
            }
            dataManager.Articles.SaveArticle(article);
            return(article);
        }


        public IActionResult AllArticles()
        {           
            var model = new ArticlesModel();
            MySessionExtensions.Set<IEnumerable<Subject>>(HttpContext.Session, KeysInTempData.Subjects, dataManager.Subjects.GetSubjects());
            model.articles = dataManager.Articles.GetArticles();
            ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("Articles");
          
            return View(model);
        }
        [HttpPost]
        public IActionResult AllArticles(ArticlesModel model)
        {
            ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("Articles");
            if (model.text == null || model.text.Trim() == "" || model.Subject == null)
            {
                
                model.articles = dataManager.Articles.GetArticles();
                return View(model);
            }

            model.articles = Search(dataManager.Articles.GetArticles(),model.text, model.Subject);
            return View(model);
        }

        public IQueryable<Article> Search(IQueryable<Article> articles, string? text, string? subject)
        {

            List<Article> Articles = new List<Article>();

            List<string> ArticlesTitles = new List<string>();
            
            if (subject != null)
            {
                foreach (var art in Articles)
                {
                    if(art.Subject == subject)
                    {
                        Articles.Add(art);
                    }
                }

            }
            else
            {
                Articles = articles.ToList();
            }
            
            if(text == null)
            {
                return Articles.AsQueryable();
            }
            
            foreach (var Article in Articles)
            {
                ArticlesTitles.Add(Article.Title);
            }

            var indexes = new List<int>();

            List<string> wordsInString = null;

            var finalArticlesTitles = new List<string>();

            

            if (text.Contains(' '))
            {
                wordsInString = text.Split(' ').ToList();
            }

            for (int i = 0; i < ArticlesTitles.Count; i++)
            {
                if (wordsInString == null)
                {
                    if (ArticlesTitles[i].ToLower().Contains(text.ToLower().Trim()))
                    {
                        indexes.Add(i);
                        finalArticlesTitles.Add(ArticlesTitles[i]);
                    }
                }
                else
                {
                    bool isWordSuit = true;

                    foreach (var word in wordsInString)
                    {
                        if (!ArticlesTitles[i].ToLower().Contains(word.ToLower().Trim()))
                        {
                            isWordSuit = false;
                        }
                    }
                    if (isWordSuit)
                        finalArticlesTitles.Add(ArticlesTitles[i]);
                    indexes.Add(i);
                }
            }



            List<Article> finalArticles = new();
            
            foreach (var articleTitle in finalArticlesTitles)
            {
                foreach (var article in Articles)
                {
                    if (article.Title == articleTitle)
                    {
                        finalArticles.Add(article);
                    }
                }
            }
            return finalArticles.AsQueryable();

        }
    }
}
