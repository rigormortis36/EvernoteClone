using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entites.Messages;
using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();

        // GET: Home
        public ActionResult Index()
        {
            //CategoryController uzerinden gelen Vıew Talebi..
            //if (TempData["mm"]!=null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}

            
            return View(noteManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            Category cat = categoryManager.Find(x => x.Id == id.Value); ;

            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index","Home");
            }

            return View("Index", cat.Notes.OrderByDescending(x=>x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
         }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;

            
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

            if(res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Ups! Bir Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;

            
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Ups! Bir Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if(ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Profil Güncellenemedi.",
                        Items = res.Errors,
                        RedirectingUrl = "/Home/EditProfile"

                    };

                    return View("Error", errorNotifyObj);
                }
                Session["login"] = res.Result; //profil güncellendiği için Session güncellendi.
                return RedirectToAction("ShowProfile");

            }
            return View(model);

        }

        public ActionResult DeleteProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            
            

            BusinessLayerResult<EvernoteUser> res =
                evernoteUserManager.RemoveUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1111-1111-11111";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                Session["login"] = res.Result;      // Session'a kullanıcı bilgilerini saklama..
                return RedirectToAction("Index");  // yönlendirme

            } 
            
            return View(model);
        }

        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                
               BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(model);

                if (res.Errors.Count >0 )
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                //EvernoteUser user = null;
                //
                //try
                //{
                //    eum.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", ex.Message);
                //    
                //}

                //    if (model.Username == "xxx")
                //{
                //    ModelState.AddModelError("", "Bu kullanıcı adı kullanılıyor.");
                //
                //}
                //
                //if (model.Email == "xxx@xxx.com")
                //{
                //    ModelState.AddModelError("", "Bu E-posta adresi kullanılıyor.");
                //
                //}
                //
                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(model);
                //    }
                //}

                //if (user == null)
                //{
                //    return View(model);
                //}

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl ="/Home/Login",
                    };

                notifyObj.Items.Add("Lütfen e-posta adresinize gönderilen aktivasyon linkine tıklayarak üyeliğinizi aktive ediniz. Üyeliğinizi aktive etmezseniz, not ekleyemez ve beğenemezsiniz.");
                return View("Ok", notifyObj);
           }
            
            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                 
                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi.",
                RedirectingUrl = "/Home/Login",
            };

            okNotifyObj.Items.Add("Hesabınız Aktifleştirildi.");
            return View("Ok", okNotifyObj);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        

    }
}