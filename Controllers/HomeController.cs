using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using FirebaseAdmin.Auth;
using KeepIt.Models;
namespace KeepIt.Controllers
{
    public class HomeController : Controller
    {
        
        DB_Entities _db = new DB_Entities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            Models.User user = new User();
            return View(user);
        }

        //POST: Register
        [HttpPost]
        public ActionResult Register(User _user, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var check = _db.Users.FirstOrDefault(s => s.Email == _user.Email);
                    if (check == null)
                    {
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        if (_user.ImageUpload != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(_user.ImageUpload.FileName);
                            string extension = Path.GetExtension(_user.ImageUpload.FileName);
                            fileName += extension;
                            _user.ImageProfile = "~/Content/Img/" + fileName;
                            _user.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Img/"), fileName));
                        }
                        Session["UserId"] = _user.UserId;
                        Session["UserName"] = _user.UserName;
                        Session["Email"] = _user.Email;
                        Session["PhoneNumber"] = _user.PhoneNumber;
                        Session["Password"] = _user.Password;
                        Session["ConfirmPassword"] = _user.ConfirmPassword;
                        Session["ImageProfile"] = _user.ImageProfile;
                        _db.Users.Add(_user);
                        _db.SaveChanges();
                        ViewBag.error = "Register Successful! Go to Log In.";
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        ViewBag.error = "Account already exists";
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var data = _db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(password)).ToList();
                if (email.Equals("admin@gmail.com") && password.Equals("admin@dm1n"))
                {
                    return Redirect("~/Admin/Admin/Index");
                }
                if (data.Count() > 0)
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                    /*var authTicket = new FormsAuthenticationTicket(1,data.FirstOrDefault().UserName,DateTime.Now,DateTime.Now.AddMinutes(20),data.FirstOrDefault().RememberMe,"","/");
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);*/

                    //add session
                    Session["UserName"] = data.FirstOrDefault().UserName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["UserId"] = data.FirstOrDefault().UserId;
                    Session["PhoneNumber"] = data.FirstOrDefault().PhoneNumber;
                    Session["Password"] = data.FirstOrDefault().Password;
                    Session["ConfirmPassword"] = data.FirstOrDefault().ConfirmPassword;
                    return RedirectToAction("Index","Login", new {UserId = data.FirstOrDefault().UserId});
                }
                else
                {
                    ViewBag.error = "Email or Password incorrect!";
                }
            }
            return View();
        }
        public ActionResult About()
        {
            return View();
        }


    }
}