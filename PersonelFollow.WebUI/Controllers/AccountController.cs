using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonelFollow.Core.Abstract;
using PersonelFollow.Entities.Concrete;
using PersonelFollow.WebUI.Filter;
using PersonelFollow.WebUI.Models;
using PersonelFollow.WebUI.Services.Session;

namespace PersonelFollow.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IUserRepository _userRepository;
        private readonly IMyActivityFollowRepository _myActivityFollowRepository;
        private readonly IActiviyRepository _activiyRepository;
        public AccountController(ISessionService sessionService, IUserRepository userRepository, IMyActivityFollowRepository myActivityFollowRepository, IActiviyRepository activiyRepository)
        {
            _sessionService = sessionService;
            _userRepository = userRepository;
            _myActivityFollowRepository = myActivityFollowRepository;
            _activiyRepository = activiyRepository;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            if (_sessionService.GetSession() == null) return View();
            return returnUrl != null ? (IActionResult)Redirect(returnUrl) : RedirectToAction("Aktivitelerim");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.returnUrl = ReturnUrl;
                return View(model);
            }
            else
            {
                var user = _userRepository.GetUser(model.EMail, model.Password);
                if (!string.IsNullOrWhiteSpace(user))
                {
                    var userInformation = _userRepository.GetUserInformation(user);
                    _sessionService.SetSession(user.ToString(),userInformation.UserName+" "+userInformation.UserSurname);
                    return ReturnUrl != null ? (IActionResult)Redirect(ReturnUrl) : RedirectToAction("Aktivitelerim");
                }
                else
                {
                    ViewBag.returnUrl = ReturnUrl;
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                    return View(model);
                }
            }
        }
        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Aktivitelerim(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Today;
            }
            else
            {
                if (date <= DateTime.Today)
                {
                    var userRegisterDate = _userRepository.GetUserInformation(_sessionService.GetSession());
                    if (userRegisterDate.UserRegisterDate > date)
                    {
                        TempData["hata"] = "Kayıt olduğunuz tarihten öncesine gidemezsiniz";
                        date = DateTime.Today;
                    }
                }
                else
                {
                    TempData["hata"] = "Bugünün tarihinden ileriye gidemezsiniz.";
                    date = DateTime.Today;
                }
            }
            Tanimla();
            var dailyActivityModel = _myActivityFollowRepository.MyActivity(Convert.ToDateTime(date), _sessionService.GetSession()).Select(
                p =>
                    new ActivitiesViewModel()
                    {
                        ActivityId = p.ActivityId,
                        DailyActivityId = p.ActivityFollowId,
                        NumberOfActivity = p.NumberOfActivities,
                        isNumeric = p.Activity.isNumeric,
                        ActivityName = p.Activity.ActivityName,
                        ActivityDate = p.ActivityDate
                    }).ToList();
            ViewBag.activityDate = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            return View(dailyActivityModel);
        }

        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult GunlukAktiviteGuncelle(List<ActivitiesViewModel> model, DateTime? date)
        {
            var userId = _sessionService.GetSession();
            String sonuc = "Kayıt başarı ile tamamlandı.";
            var activities = new List<ActivityFollow>();
            foreach (var t in model)
            {
                if (!(t.NumberOfActivity < 0))
                {
                    if (_myActivityFollowRepository.HasActivity(t.DailyActivityId, userId))
                    {
                        activities.Add(new ActivityFollow()
                        {
                            ActivityFollowId = t.DailyActivityId,
                            NumberOfActivities = t.NumberOfActivity
                        });
                    }
                    else
                    {
                        sonuc +=
                            " Dikkat! Size ait olmayan bir aktivite günlüğünü güncellemeye çalıştınız.";
                    }
                }
                else
                {
                    sonuc +=
                        " Dikkat! Sıfırdan küçük girdiğiniz değer sisteme eklenmedi. Lütfen düzeltip tekrar deneyiniz.";
                }

                TempData["basari"] = sonuc;
            }
            _myActivityFollowRepository.Save(activities);
            return RedirectToAction("Aktivitelerim", new { date });
        }

        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult AktiviteListem()
        {
            var aktiviteListesi = _activiyRepository.GetActivities(_sessionService.GetSession()).
                Select(p => new NewAndEditActivityViewModel()
                {
                    ActivityId = p.ActivityId,
                    ActivityName = p.ActivityName,
                    IsNumeric = p.isNumeric,
                    IsActive = p.isActive
                }).ToList();
            return View(aktiviteListesi);
        }



        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Yeni()
        {
            Tanimla();
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Yeni(NewAndEditActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _activiyRepository.Add(new Activity()
                {
                    ActivityName = model.ActivityName,
                    isNumeric = model.IsNumeric,
                    UserId = new Guid(_sessionService.GetSession())
                });
                if (result)
                {
                    TempData["basari"] = "Yeni aktivite başarı ile oluşturuldu. Oluşturulduğu tarihten itibaren artık günlüklerinize eklenecektir.";
                    return RedirectToAction("AktiviteListem");
                }
                else
                {
                    TempData["hata"] = "Aynı isimden ve aynı türden bir aktivite zaten var";
                    Tanimla();
                    return View(model);
                }
            }
            else
            {
                Tanimla();
                return View(model);
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Duzenle(int? id)
        {
            if (id != null)
            {
                var result = _activiyRepository.HasActivity(id.Value, _sessionService.GetSession());
                if (result)
                {
                    Tanimla();
                    var activity = _activiyRepository.GetActivity(id.Value, _sessionService.GetSession());
                    return View(new NewAndEditActivityViewModel()
                    {
                        IsActive = activity.isActive,
                        ActivityName = activity.ActivityName,
                        ActivityId = activity.ActivityId,
                        UserId = activity.UserId.ToString(),
                        IsNumeric = activity.isNumeric
                    });
                }
                else
                {
                    TempData["hata"] = "Yalnızca kendi aktivitelerinizi düzenleyebilirsiniz.";
                    return RedirectToAction("AktiviteListem");
                }
            }
            else
            {
                TempData["hata"] = "Lütfen düzenlemek istediğiniz aktiviteyi seçiniz.";
                return RedirectToAction("AktiviteListem");
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Duzenle(NewAndEditActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_activiyRepository.HasActivity(model.ActivityId, model.UserId))
                {
                    var result = _activiyRepository.Update(new Activity()
                    {
                        ActivityName = model.ActivityName,
                        isNumeric = model.IsNumeric,
                        isActive = model.IsActive,
                        ActivityId = model.ActivityId,
                        UserId = new Guid(model.UserId)
                    });
                    if (result)
                    {
                        TempData["basari"] = "Aktivite düzenleme işlemi başarı ile sonuçlandı.";
                        return RedirectToAction("AktiviteListem");
                    }
                    else
                    {
                        TempData["hata"] =
                            "Aynı özelliklere ve aynı isme sahip farklı bir aktiviteye zaten sahipsiniz.";
                        Tanimla();
                        return View(model);
                    }
                }
                else
                {
                    TempData["hata"] = "Ylnızca kendinize ait olan aktiviteleri düzenleyebilirsiniz.";
                    return RedirectToAction("AktiviteListem");
                }
            }
            else
            {
                Tanimla();
                return View(model);
            }
        }


        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                if (_activiyRepository.HasActivity(id.Value, _sessionService.GetSession()))
                {
                    _activiyRepository.Delete(new Activity()
                    {
                        ActivityId = id.Value
                    });
                    TempData["basari"] = "Aktivite devre dışı bırrakma işlemi başarı ile sonuçlandı. Artık aktivite listelerde dahil olmayacaktır.";
                    return RedirectToAction("AktiviteListem");
                }
                else
                {
                    TempData["hata"] = "Yalnızca kendi aktiviteleriniz için işlem yapabilirsiniz.";
                    return RedirectToAction("AktiviteListem");
                }
            }
            else
            {
                TempData["hata"] = "Lütfen devre dışı bırakmak istediğiniz aktiviteyi seçiniz.";
                return RedirectToAction("AktiviteListem");
            }
        }

        public IActionResult KayitOl()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult KayitOl(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _userRepository.Add(new UserInformation()
                {
                    EMail = model.EMail,
                    Password = model.Password,
                    UserName = model.UserName,
                    UserSurname = model.UserSurname
                });
                if (result)
                {
                    TempData["basari"] = "Kayıt başarı ile tamamlandı. Lütfen giriş yapınız.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["hata"] =
                        "Aynı E-Posta hesabına sahip farklı bir hesap bulunmaktadır. Lütfen E-Posta adresini değiştiriniz.";
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult ProfilimiGuncelle()
        {
            var user = _userRepository.GetUserInformation(_sessionService.GetSession());
            if (user != null)
            {
                return View(new UserProfileEditModel()
                {
                    EMail = user.EMail,
                    UserName = user.UserName,
                    UserSurname = user.UserSurname
                });
            }
            else
            {
                TempData["hata"] = "Bilgileriniz bulunamadı.";
                return View();
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(LoginFilter))]
        [ValidateAntiForgeryToken]
        public IActionResult ProfilimiGuncelle(UserProfileEditModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetUserInformation(_sessionService.GetSession());
                if (user != null)
                {
                    SHA1 sha = new SHA1CryptoServiceProvider();
                    var sha1Password = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(model.OldPassword)));
                    if (user.Password == sha1Password)
                    {
                        var result = _userRepository.Update(new UserInformation()
                        {
                            UserId = new Guid(_sessionService.GetSession()),
                            EMail = model.EMail,
                            Password = model.Password,
                            UserSurname = model.UserSurname,
                            UserName = model.UserName
                        });
                        if (result)
                        {
                            TempData["basari"] = "Profil güncelleme işlemi başarı ile tamamlandı.";
                            return RedirectToAction("Aktivitelerim");
                        }
                        else
                        {
                            TempData["hata"] =
                                "Güncellemek istediğiniz E-Posta adresi kullanılmaktadır. Lütfen E-Posta adresinizi değiştiriniz.";
                            return View(model);
                        }
                    }
                    else
                    {
                        TempData["hata"] = "Eski şifre hatalı girildi.";
                        return View(model);
                    }
                }
                else
                {
                    TempData["hata"] = "Sistemden silinmiş bir kullanıcı için işlem yapmaya çalıştınız.";
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }


        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult SignOut()
        {
            _sessionService.DeleteSession();
            return RedirectToAction("Login");
        }
        public void Tanimla()
        {
            var yapildiYapilmadiSelectList = new List<SelectListItem>()
            {
                new SelectListItem("Yapılmadı", "0"),
                new SelectListItem("Yapıldı", "1")
            };
            ViewBag.yapildiYapilmadiSelect = yapildiYapilmadiSelectList;
            var aktiviteTuru = new List<SelectListItem>()
            {
                new SelectListItem("Yapıldı Yapılmadı", "false"),
                new SelectListItem("Sayısal Değer", "true")
            };
            ViewBag.aktiviteTuruSelect = aktiviteTuru;
            var aktiflikDurumu = new List<SelectListItem>()
            {
                new SelectListItem("Aktif Değil", "false"),
                new SelectListItem("Aktif", "true")
            };
            ViewBag.aktiflikDurumu = aktiflikDurumu;
        }
    }
}