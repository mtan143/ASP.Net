using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KeepIt.Models;

namespace KeepIt.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        DB_Entities _db = new DB_Entities();

        /*[HttpGet]
        public ActionResult Index()
        {
            return View(Models.User);
        }*/
        [HttpGet]
        public ActionResult Index(int UserId)
        {
            return View(_db.Users.Where(s => s.UserId == UserId).FirstOrDefault());
        }
        public ActionResult Logout()
        {
            //Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public new ActionResult Profile(int UserId)
        {
            return View(_db.Users.Where(s=>s.UserId == UserId).FirstOrDefault());
        }

        [HttpGet]
        public ActionResult Update(int UserId)
        {
            return View(_db.Users.Where(s => s.UserId == UserId).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Update(User _user)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(_user).State = System.Data.Entity.EntityState.Modified;/*
                var check = _db.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {*/
                    Session["UserId"] = _user.UserId;
                    Session["UserName"] = _user.UserName;
                    Session["Email"] = _user.Email;
                    Session["PhoneNumber"] = _user.PhoneNumber;
                    Session["Password"] = _user.Password;
                    Session["ConfirmPassword"] = _user.ConfirmPassword;
                    _db.SaveChanges();
                    ViewBag.message = "true";
                /*}
                else
                {
                    ViewBag.error = "Account already exists";
                }*/
            }
            return View();
        }
    }
}