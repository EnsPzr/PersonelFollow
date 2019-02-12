using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonelFollow.Core.Abstract;
using PersonelFollow.Core.Concrete.EntityFramework;
using PersonelFollow.WebUI.Filter;
using PersonelFollow.WebUI.Models;
using PersonelFollow.WebUI.Services.Session;

namespace PersonelFollow.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IUserRepository _userRepository;
        private static ISessionService _sessionService;

        public HomeController(IUserRepository userRepository, ISessionService sessionService)
        {
            _userRepository = userRepository;
            _sessionService = sessionService;
        }

        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Index(DateTime? date)
        {
            return View(_userRepository.GetAllUser());
        }

        public IActionResult Giris(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetUser(model.EMail, model.Password);
                if (user != null)
                {
                    //_sessionService.SetSession(user.ToString());
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("","Kullanıcı adı veya şifre hatalı");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
