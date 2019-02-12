using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonelFollow.WebUI.Services.Session;

namespace PersonelFollow.WebUI.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly ISessionService _sessionService;

        public NavbarViewComponent(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.GirisVarMi = _sessionService.GetSessionUserInformation() != null;
            ViewBag.UserName = _sessionService.GetSessionUserInformation();
            return View("Default");
        }
    }
}
