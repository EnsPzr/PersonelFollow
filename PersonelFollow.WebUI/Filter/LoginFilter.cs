using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using PersonelFollow.Core.Abstract;
using PersonelFollow.WebUI.Services.Session;

namespace PersonelFollow.WebUI.Filter
{
    public class LoginFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly ISessionService _sessionService;
        private IUserRepository _userRepository;



        public LoginFilter(ISessionService sessionService, IUserRepository userRepository)
        {
            _sessionService = sessionService;
            _userRepository = userRepository;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            var tem = context.HttpContext.RequestServices.GetService<ITempDataDictionary>();
            var factory = context.HttpContext?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = factory?.GetTempData(context.HttpContext);
            var session = _sessionService.GetSession();
            if (session != null)
            {
                var user = _userRepository.GetUserInformation(session.ToString());
                if (user == null)
                {
                    if (tempData != null) tempData["hata"] = "Geçersiz bir kullanıcı ile giriş yapılmaya çalışıldı.";
                    context.Result = new RedirectToActionResult("Login", "Account",new{ returnUrl = "~/" + controllerName + "/" + actionName });
                }

            }
            else
            {
                if (tempData != null) tempData["hata"] = "Oturum zaman aşımına uğradı.";
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = "~/" + controllerName + "/" + actionName });
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            var tem = context.HttpContext.RequestServices.GetService<ITempDataDictionary>();
            var factory = context.HttpContext?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = factory?.GetTempData(context.HttpContext);
            var session = _sessionService.GetSession();
            if (session != null)
            {
                var user = _userRepository.GetUserInformation(session.ToString());
                if (user == null)
                {
                    if (tempData != null) tempData["hata"] = "Geçersiz bir kullanıcı ile giriş yapılmaya çalışıldı.";
                    context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = "~/" + controllerName + "/" + actionName });
                }

            }
            else
            {
                if (tempData != null) tempData["hata"] = "Oturum zaman aşımına uğradı.";
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = "~/" + controllerName + "/" + actionName });
            }
        }
    }
}
