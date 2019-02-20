using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCV_Project.Models;
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
                if (ModelState.IsValid)
                {
                    db.StoreHouses.Add(storeHouse);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return View();
            }
        }

        public ActionResult Edit(int? id)
        {

            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
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
                TempData["SHId"] = id; 
                return RedirectToAction("Index", "Rack", new { iD = id });
            }
        }

    }
}