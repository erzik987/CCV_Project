using System;
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
    public class RackModel
    {
        public List<Rack> Racks { get; set; }
        public int StoreHouseId { get; set; }
    }

    public class RackController : Controller
    {
        public ActionResult Index(int id)
        {
            Session["StoreHouseID"] = id;
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["StoreHouseName"] = db.StoreHouses.Where(c => c.StoreHouseId == id).Single().StoreHouseName;
                List<Rack> racks = db.Racks.Where(c=>c.StoreHouseRefId == id).ToList();
                var model = new RackModel(){Racks = racks,StoreHouseId = id};

                return View(model);
            }
        }

        public ActionResult NewRack()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewRack(Rack rack)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                if (ModelState.IsValid)
                {
                    bool isAlreadyRegistered = db.Racks.Any(c => c.Xposition == rack.Xposition)&& db.Racks.Any(c => c.Yposition == rack.Yposition) && db.Racks.Any(c => c.StoreHouseRefId == rack.StoreHouseRefId);
                    if (!isAlreadyRegistered)
                    {
                        rack.StoreHouseRefId = (int)HttpContext.Session["StoreHouseID"];
                        db.Racks.Add(rack);
                        db.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("Index", new { iD = Session["StoreHouseID"] });
                    }
                    else
                    {
                        TempData["AlreadyExist"] = "You can not have two rack on the same position";
                    }
                }
                return View();
            }
        }

        public ActionResult Create()
        {
            return RedirectToAction("NewRack");
        }

        public ActionResult Edit(int? id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["RackName"] = db.Racks.Where(c => c.RackId == id).Single().Xposition + ":" + db.Racks.Where(c => c.RackId == id).Single().Yposition;
                if (id == null)
                {
                    return new HttpStatusCodeResult(BadRequest);
                }
                Rack rack = db.Racks.Find(id);
                if (rack == null)
                {
                    return HttpNotFound();
                }

                return View(db.Racks.SingleOrDefault(c => c.RackId == id));
            }
        }

        [HttpPost]
        public ActionResult Edit(Rack rack)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                if (ModelState.IsValid)
                {
                    rack.StoreHouseRefId = (int)HttpContext.Session["StoreHouseID"];
                    db.Entry(rack).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new{iD = Session["StoreHouseID"]});
                }

                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["RackName"] = db.Racks.Where(c => c.RackId == id).Single().Xposition + ":" + db.Racks.Where(c => c.RackId == id).Single().Yposition;
                return View(db.Racks.SingleOrDefault(c => c.RackId == id));
            }
        }

        public ActionResult Delete(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                var rack = db.Racks.SingleOrDefault(c => c.RackId == id);
                db.Racks.Remove(rack);
                db.SaveChanges();
                return RedirectToAction("Index",new {iD = Session["StoreHouseID"]});
            }
        }

        public ActionResult More(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["RackName"] = db.Racks.Where(c => c.RackId == id).Single().Xposition + ":" + db.Racks.Where(c => c.RackId == id).Single().Yposition;
                return RedirectToAction("Index", "Shelf", new { iD = id });
            }
        }

        public ActionResult BackToStoreHouseIndex()
        {
            return RedirectToAction("Index", "StoreHouse");
        }

        public ActionResult SingOut()
        {
            Session["IsAdmin"] = false;
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
                                Rack rack = new Rack();
                                rack.EAN = Int32.Parse(words[0]);
                                rack.Xposition = Int32.Parse(words[1]);
                                rack.Yposition = Int32.Parse(words[2]);
                                rack.Activ = Boolean.Parse(words[3]);
                                rack.StoreHouseRefId = (int)Session["StoreHouseID"];
                                db.Racks.Add(rack);
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