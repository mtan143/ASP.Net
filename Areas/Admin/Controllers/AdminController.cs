using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeepIt.Models;

namespace KeepIt.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        DB_Entities _db = new DB_Entities();


        // GET: Admin/Home
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Home/Details/5
        public ActionResult ManageUser()
        {
            var data = _db.Users.ToList();
            return View(data);
        }

        // GET: Admin/Home/Create
        public ActionResult Create()
        {
            KeepIt.Models.User user = new User();
            return View(user);
        }

        //POST: Register
        [HttpPost]
        public ActionResult Create(User _user, HttpPostedFileBase file)
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
                    return RedirectToAction("ManageUser", "Admin");
                }
                else
                {
                    ViewBag.error = "Account already exists";
                }
            }
            return View();
        }

        //Update: 
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

        // GET: Admin/Home/Delete/5
        [HttpGet]
        public ActionResult Delete(int UserId)
        {
            return View(_db.Users.Where(s=>s.UserId ==UserId).FirstOrDefault());
        }

        // POST: Admin/Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int UserId, User user)
        {
            var data = _db.Users.Where(x => x.UserId == UserId).FirstOrDefault();
            if (data != null)
            {
                _db.Users.Remove(data);
                _db.SaveChanges();
                return RedirectToAction("ManageUser", "Admin");
            }
            else return RedirectToAction("ManageUser", "Admin");
        }
    }
}
