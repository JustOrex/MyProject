using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Hosting.Internal;
using MyOpinion.Domain;
using MyOpinion.Domain.Entities;
using MyOpinion.Service;

namespace MyOpinion.Areas.User.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataManager dataManager;

        public ProfileController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult GetUser(string id)
        {
            var entity = id == default ? new IdentityUser() : dataManager.Users.GetUserById(id.ToString());
            return View(entity);
        }
        [HttpPost]
        public IActionResult Edit(IdentityUser model)
        {
            if (ModelState.IsValid)
            {
                dataManager.Users.SaveUser(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }
    }
}
