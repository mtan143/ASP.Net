
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeepIt.Models;

namespace KeepIt.Controllers
{
    [Authorize]
    public class ObjectController : Controller
    {
        DB_Entities _db = new DB_Entities(); 

        // GET: Object/Details/5
        [HttpGet]
        public ActionResult Show()
        {
            return View(_db.Objects.ToList());
        }

        // GET: Object/CreateProfile
        public ActionResult CreateProfile()
        {
            Models.Object obj = new Models.Object();
            return View(obj);
        }

        // GET: Object/CreateImage
        public ActionResult CreateImage()
        {
            Models.Object obj = new Models.Object();
            return View(obj);
        }

        [HttpPost]
        public ActionResult CreateImage(HttpPostedFileBase file, KeepIt.Models.Object objects)
        {
            try
            {
                objects.UserId = (int)Session["UserId"];
                if (objects.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objects.ImageUpload.FileName);
                    string extension = Path.GetExtension(objects.ImageUpload.FileName);
                    fileName += extension;
                    objects.ImageName = "~/Content/Img/" + fileName;
                    objects.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Img/"), fileName));
                }
                _db.Objects.Add(objects);
                _db.SaveChanges();
                return RedirectToAction("Show", "Object");
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult CreateProfile(HttpPostedFileBase file, KeepIt.Models.Object objects)
        {
            // TODO: Add insert logic here
            try 
            {
                objects.UserId = (int)Session["UserId"];
                if (objects.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objects.ImageUpload.FileName);
                    string extension = Path.GetExtension(objects.ImageUpload.FileName);
                    fileName += extension;
                    objects.ImageName = "~/Content/Img/" + fileName;
                    objects.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Img/"), fileName));
                }
                _db.Objects.Add(objects);
                _db.SaveChanges();
                return RedirectToAction("Show", "Object");
            }
            catch 
            {
                return View();
            }


        }

        [HttpPost]
        public ActionResult ProfileImage(HttpPostedFileBase file ,KeepIt.Models.Object objects)
        {
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            string extention = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/ImageProfile/"), _filename);
            objects.ImageName = "~/ImageProfile/" + _filename;

            if(extention.ToLower() == ".jpg" || extention.ToLower() == ".jpeg"|| extention.ToLower() == ".png")
            {
                if (file.ContentLength <= 1000000)
                {
                    _db.Objects.Add(objects);
                    if (_db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        ViewBag.message = "Add Image Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.message = "Invalid Image";
                }
            }
            return View();
        }

        //Update: 
        [HttpGet]
        public ActionResult Edit(int ObjectId)
        {
            return View(_db.Objects.Where(s => s.ObjectId == ObjectId).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(KeepIt.Models.Object _obj)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(_obj).State = System.Data.Entity.EntityState.Modified;/*
                var check = _db.Objects.FirstOrDefault(s => s.PhoneNumber == _obj.PhoneNumber);
                if (check == null)
                {*/
                    Session["ObjectId"] = _obj.ObjectId;
                    Session["ImageName"] = _obj.ImageName;
                    Session["FullName"] = _obj.FullName;
                    Session["Gender"] = _obj.Gender;
                    Session["DateOfBirth"] = _obj.DateOfBirth;
                    Session["PlaceOfBirth"] = _obj.PlaceOfBirth;
                    Session["Address"] = _obj.Address;
                    Session["Email"] = _obj.Email;
                    Session["PhoneNumber"] = _obj.PhoneNumber;
                    Session["MaterialStatus"] = _obj.MaterialStatus;
                    Session["Career"] = _obj.Career;
                    Session["Education"] = _obj.Education;
                    Session["Skill"] = _obj.Skill;
                    Session["Hobby"] = _obj.Hobby;
                    Session["UserId"] = _obj.UserId;
                    _db.SaveChanges();
                    ViewBag.message = "true";
                /*}
                else
                {
                    ViewBag.message = "false";
                }*/
            }
            return View();
        }


        [HttpGet]
        public ActionResult Delete(int ObjectId)
        {
            return View(_db.Objects.Where(s => s.ObjectId == ObjectId).FirstOrDefault());
        }

        // POST: Admin/Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int ObjectId, KeepIt.Models.Object obj)
        {
            var data = _db.Objects.Where(x => x.ObjectId == ObjectId).FirstOrDefault();
            if (data != null)
            {
                _db.Objects.Remove(data);
                _db.SaveChanges();
                return RedirectToAction("Show", "Object");
            }
            else return RedirectToAction("Show", "Object");
        }
    }
}
