﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCV_Project.Models;
using static System.Net.HttpStatusCode;


namespace CCV_Project.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                return View(db.UserAcounts.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAcount account)
        {
            if (ModelState.IsValid)
            {
                using (CCV_Tables_Context db = new CCV_Tables_Context())
                {

                    bool isAlreadyRegistered = db.UserAcounts.Any(c => c.Password == account.Password)&&db.UserAcounts.Any(c=>c.Username == account.Username);
                    if (!isAlreadyRegistered)
                    {
                        db.UserAcounts.Add(account);
                        db.SaveChanges();
                        ModelState.Clear();
                        TempData["WelcomeUser"] = account.LastName + " " + account.FirstName + " successfully registered";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["AlreadyExist"] = "Please change your password or username";
                    }
                    
                }
                ViewBag.Message = account.LastName + " " + account.FirstName + " successfully registered";
            }

            return View();
        }

        public ActionResult Login()
        {
            Session["IsAdmin"] = false;
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAcount user)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                bool validEmail = db.UserAcounts.Any(x => x.Username == user.Username);
                bool validPassword = db.UserAcounts.Any(x => x.Password == user.Password);

                if (!validEmail)
                {
                    TempData["alertMessage"] = "Wrong username or password";
                    return RedirectToAction("Login");
                }

                if (!validPassword)
                {
                    TempData["alertMessage"] = "Wrong username or password";
                    return RedirectToAction("Login");
                }

                var usr = db.UserAcounts.Single(u => u.Username == user.Username && u.Password == user.Password);
                if (usr != null)
                {
                    Session["UserID"] = usr.UserId.ToString();
                    Session["FirstName"] = usr.FirstName.ToString();
                    bool admin = (usr.UserId == 1);
                    if (admin)
                    {
                        Session["IsAdmin"] = true;
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Ussername or Password is wrong");
                }
            }

            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "StoreHouse", new { area = "" });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Create()
        {
            return RedirectToAction("Register");
        }

        public ActionResult Edit(int? id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(BadRequest);
                }
                UserAcount account = db.UserAcounts.Find(id);
                if (User == null)
                {
                    return HttpNotFound();
                }
                return View(db.UserAcounts.SingleOrDefault(c => c.UserId == id));
            }
        }

        [HttpPost]
        public ActionResult Edit(UserAcount account)
        {
            if (ModelState.IsValid)
            {
                using (CCV_Tables_Context db = new CCV_Tables_Context())
                {

                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                return View(db.UserAcounts.SingleOrDefault(c => c.UserId == id));
            }  
        }

        public ActionResult Delete(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                 var emp = db.UserAcounts.SingleOrDefault(c => c.UserId == id);
                db.UserAcounts.Remove(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            } 
        }

        public ActionResult SingOut()
        {
            Session["IsAdmin"] = false;
            return RedirectToAction("SingOut", "Shared");
        }
    }
}