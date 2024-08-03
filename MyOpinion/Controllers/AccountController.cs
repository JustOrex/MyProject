
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyOpinion.Models;
using MyOpinion.Services;
using Microsoft.Extensions.Caching.Memory;
using MyOpinion.Domain;
using MyOpinion.Service;

namespace MyOpinion.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly DataManager dataManager;

        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signinMgr, DataManager dataMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            dataManager = dataMgr;
        }
        
        
        [AllowAnonymous]      
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var dic = new Dictionary<string, string>();
                        dic.Add(KeysInTempData.UserName,user.UserName);
                        dic.Add(KeysInTempData.Email, user.Email);
                        dic.Add(KeysInTempData.Password, model.Password);
                        dic.Add(KeysInTempData.Id, user.Id);                        
                        MySessionExtensions.Set(HttpContext.Session,KeysInTempData.Dictionary, dic);
                        
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неверный логин или пароль");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signup() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityUser NameTest = await userManager.FindByNameAsync(model.UserName);
                
                if (NameTest == null) 
                {
                                                                                            
                    var dic = new Dictionary<string, string>();
                    dic.Add(KeysInTempData.UserName, model.UserName);
                    //dic.Add(KeysInTempData.Email, model.Email);
                    dic.Add(KeysInTempData.Password, model.Password);
                    dic.Add(KeysInTempData.Id, Guid.NewGuid().ToString());

                    MySessionExtensions.Set(HttpContext.Session, KeysInTempData.Dictionary, dic);
                    var response = await CreateUser();

                    if (response.State)
                    {
                        HttpContext.Session.SetString(KeysInTempData.Signup, "null");
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), "Что-то пошло не так");
                    return View(model);


                }
                ModelState.AddModelError(nameof(SignupViewModel.UserName), "Пользователь с таким логином уже существует");
                return View(model);
            }
            return View(model);
        }


        //public IActionResult ConfirmEmail() => View();

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string code = HttpContext.Session.GetString(KeysInTempData.Code);
        //        if (model.Code == code)
        //        {
        //            if (HttpContext.Session.GetString(KeysInTempData.Signup) == KeysInTempData.Signup)
        //            {

        //                var response = await CreateUser();

        //                if (response.State)
        //                {
        //                    HttpContext.Session.SetString(KeysInTempData.Signup, "null");
        //                    return RedirectToAction("Index", "Home");
        //                }
        //                ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), "Что-то пошло не так");
        //                return View(model);
        //            }
        //            if (HttpContext.Session.GetString(KeysInTempData.EditEmail) == KeysInTempData.EditEmail)
        //            {

        //                return RedirectToAction("EditEmail", "Account");
        //            }
        //            if (HttpContext.Session.GetString(KeysInTempData.ConfirmEmail) == KeysInTempData.ConfirmEmail)
        //            {
        //                HttpContext.Session.SetString(KeysInTempData.ConfirmEmail, "null");
        //                var newEmail = HttpContext.Session.GetString(KeysInTempData.EmailToChange);
        //                var data = new ChangeViewModel();
        //                data.Email = newEmail;
        //                HttpContext.Session.SetString(KeysInTempData.WhatChanging, KeysInTempData.Email);
        //                var response = await EditUser(data);

        //                if (response.State)
        //                {
        //                    return RedirectToAction("Index", "Home");
        //                }
        //                ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), "Что-то пошло не так");
        //            }

        //        }
        //        ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), "Неверный код");
        //    }
        //    return View(model);
        //}
        [HttpGet]
		
		public IActionResult Profile() => View();
		[Authorize]
		public IActionResult Profile(string id)
		{
			var entity = id == default ? new IdentityUser() : dataManager.Users.GetUserById(id.ToString());
			return View(entity);
		}
        
        [Authorize]
        public IActionResult EditUserName(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new ChangeViewModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditUserName(ChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var response = await EditUser(model);
                if (response.State)
                {
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    if (response.Error != null)
                    {
                        ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), MySessionExtensions.Errors[4]);
                        return View(model);
                    }
                    ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), "Что-то пошло не так");
                }
            }
            return View(model);
        }

        [Authorize]
        public IActionResult EditEmail(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new ChangeViewModel());
        }
        
        
        //public async Task<IActionResult> EditEmail(ChangeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var testuser = await userManager.FindByEmailAsync(model.Email);
        //        if (testuser != null)
        //        {
        //            ModelState.AddModelError(nameof(ChangeViewModel.Email), MySessionExtensions.Errors[5]);
        //            return View(model);
        //        }
        //        await SendEmail(model.Email, "Подтверждение");
        //        HttpContext.Session.SetString(KeysInTempData.EmailToChange, model.Email);
        //        HttpContext.Session.SetString(KeysInTempData.ConfirmEmail, KeysInTempData.ConfirmEmail);
        //        HttpContext.Session.SetString(KeysInTempData.EditEmail, "null");
        //        return RedirectToAction("ConfirmEmail", "Account");
                //var response = await EditUser(model);
                //if (response.State)
                //{
                //    return RedirectToAction("Profile", "Account");
                //}
                //else
                //{
                //    if (response.Error != null)
                //    {
                //        ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), response.Error);
                //        return View(model);
                //    }
                //    ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), "Что-то пошло не так");
                //}
        //    }
        //    return View(model);
        //}

        [Authorize]
        public IActionResult EditPassword(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new ChangeViewModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPassword(ChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var response = await EditUser(model);
                if (response.State)
                {
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    if(response.Error != null)
                    {
                        ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), response.Error);
                    }
                    ModelState.AddModelError(nameof(ConfirmEmailViewModel.Code), "Что-то пошло не так");
                }
            }
            return View(model);
        }

        
        public async Task<BaseResponse> EditUser(ChangeViewModel model)
        {
            var response = new BaseResponse();
            
            Dictionary<string, string> dic = MySessionExtensions.Get<Dictionary<string, string>>(HttpContext.Session, KeysInTempData.Dictionary);
            var User = await userManager.FindByNameAsync(dic[KeysInTempData.UserName]);
            string whatchanging = HttpContext.Session.GetString(KeysInTempData.WhatChanging);
            if (whatchanging == KeysInTempData.UserName) 
            {
                var testuser = await userManager.FindByNameAsync(model.UserName);
                if (testuser != null)
                {
                    response.Error = MySessionExtensions.Errors[4];
                    return response;
                }
                User.UserName = model.UserName;
                dic[KeysInTempData.UserName] = model.UserName;
            }
            
            else if(whatchanging == KeysInTempData.Password)
            {
                if(model.OldPassword != dic[KeysInTempData.Password])
                {
                    response.Error = MySessionExtensions.Errors[1];
                }
                
                if(model.OldPassword == model.NewPassword)
                {
                    response.Error = MySessionExtensions.Errors[3];
                    return response;
                }
                User.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, model.NewPassword);
                dic[KeysInTempData.Password] = model.NewPassword;
            }

            IdentityResult result = await userManager.UpdateAsync(User); 
            if (result.Succeeded)
            {
                response.State = true;
            }

            MySessionExtensions.Set(HttpContext.Session, KeysInTempData.Dictionary, dic);

            return response;
        }
        public async Task<BaseResponse> CreateUser()
        {
            var mainResponse = new BaseResponse();

            Dictionary<string, string> dic = MySessionExtensions.Get<Dictionary<string, string>>(HttpContext.Session, KeysInTempData.Dictionary);          
            IdentityUser User = new IdentityUser(dic[KeysInTempData.UserName]);

            User.Id = dic[KeysInTempData.Id];
            User.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, dic[KeysInTempData.Password]);
            User.Email = "p@mail.com";
            User.EmailConfirmed = true;

            var response = await userManager.CreateAsync(User);
            if (!response.Succeeded)
            {
                mainResponse.Error = MySessionExtensions.Errors[0];

                return mainResponse;
            }
            

            await signInManager.SignOutAsync();
            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(User, dic[KeysInTempData.Password], true, false); ;
            if (result.Succeeded)
            {
                mainResponse.State = true;
                return mainResponse;
            }
            else
            {
                mainResponse.Error = MySessionExtensions.Errors[0];
                return mainResponse;
            }
        }
        public async Task SendEmail(string email, string title)
        {
            var es = new EmailSender(email, title);
            await es.SendEmailAsync();
            HttpContext.Session.SetString(KeysInTempData.Code, es.Code);
        }

        public async Task<IActionResult> SEforPreChanging()
        {
            Dictionary<string, string> dic = MySessionExtensions.Get<Dictionary<string, string>>(HttpContext.Session, KeysInTempData.Dictionary);
            var User = await userManager.FindByNameAsync(dic[KeysInTempData.UserName]);
            await SendEmail(User.Email, "Подтверждение");
            HttpContext.Session.SetString(KeysInTempData.EditEmail, KeysInTempData.EditEmail);
            
            return RedirectToAction("ConfirmEmail", "Account");
        }

            [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}