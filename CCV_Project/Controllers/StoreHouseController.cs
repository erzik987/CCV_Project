﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCV_Project.Classes;
using CCV_Project.Models;
using LumenWorks.Framework.IO.Csv;
using static System.Net.HttpStatusCode;

namespace CCV_Project.Controllers
{
    public class StoreHouseController : Controller
    {
        // GET: StoreHouse
        public ActionResult Index()
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                return View(db.StoreHouses.ToList());
            }
        }

        public ActionResult NewStoreHouse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewStoreHouse(StoreHouse storeHouse)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                bool isAlreadyRegistered = db.StoreHouses.Any(c => c.StoreHouseName == storeHouse.StoreHouseName);
                if (ModelState.IsValid)
                {
                    if (!isAlreadyRegistered)
                    {
                        db.StoreHouses.Add(storeHouse);
                        db.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["AlreadyExist"] = "There can not be two storehouses with same name";
                    }
                    
                }
                
                return View();
            }
        }

        public ActionResult Edit(int? id)
        {

            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["StoreHouseName"] = db.StoreHouses.Where(c => c.StoreHouseId == id).Single().StoreHouseName;
                if (id == null)
                {
                return new HttpStatusCodeResult(BadRequest);
                }
                StoreHouse account = db.StoreHouses.Find(id);
                if (User == null)
                {
                    return HttpNotFound();
                }
    
                return View(db.StoreHouses.SingleOrDefault(c => c.StoreHouseId == id));
            }
            
        }

        [HttpPost]
        public ActionResult Edit(StoreHouse storeHouse)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {

                if (ModelState.IsValid)
                {
                    db.Entry(storeHouse).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View();
            }
        }


        public ActionResult Details(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["StoreHouseName"] = db.StoreHouses.Where(c => c.StoreHouseId == id).Single().StoreHouseName;
                return View(db.StoreHouses.SingleOrDefault(c => c.StoreHouseId == id));
            }
        }

        public ActionResult Delete(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                var stHouse = db.StoreHouses.SingleOrDefault(c => c.StoreHouseId == id);
                db.StoreHouses.Remove(stHouse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Create()
        {
            return RedirectToAction("NewStoreHouse");
        }

        public ActionResult More(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["StoreHouseName"] = db.StoreHouses.Where(c => c.StoreHouseId == id).Single().StoreHouseName;
                //TempData["SHId"] = id; 
                return RedirectToAction("Index", "Rack", new { iD = id });
            }
        }

        public ActionResult Back()
        {
            return RedirectToAction("Index", "Account");
        }

        public ActionResult SingOut()
        {
            return RedirectToAction("SingOut", "Shared");
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    if (upload.FileName.EndsWith(".csv"))
                    {
                        Stream stream = upload.InputStream;
                        DataTable csvTable = new DataTable();
                        using (CsvReader csvReader =
                            new CsvReader(new StreamReader(stream), true))
                        {
                            csvTable.Load(csvReader);
                        }

                        using (CCV_Tables_Context db = new CCV_Tables_Context())
                        {
                            string[] words;
                            for (int i = 0; i < csvTable.Rows.Count; i++)
                            {
                                csvTable.Rows[i].ItemArray[0].ToString();
                                words = csvTable.Rows[i].ItemArray[0].ToString().Split(';');
                                StoreHouse storeHouse = new StoreHouse();
                                storeHouse.StoreHouseName = words[0];
                                StringToEnum myIntToEnum = new StringToEnum();
                                storeHouse.storeHouseType = myIntToEnum.GetStoreHouseTypeFromString(words[1]);
                                storeHouse.StoreHouseActiv = Boolean.Parse(words[2]);
                                db.StoreHouses.Add(storeHouse);
                                db.SaveChanges();
                            }
                        }
                        return View(csvTable);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View();
        }
    }
}