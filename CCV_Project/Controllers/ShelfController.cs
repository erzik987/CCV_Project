using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCV_Project.Models;
using LumenWorks.Framework.IO.Csv;
using static System.Net.HttpStatusCode;


namespace CCV_Project.Controllers
{

    public class ShelfModel
    {
        public List<Shelf> Shelves { get; set; }
        public int RackId { get; set; }
    }

    public class ShelfController : Controller
    {
        // GET: Shelf
        public ActionResult Index(int id)
        {
            Session["RackID"] = id;
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["RackName"] = db.Racks.Where(c => c.RackId == id).Single().Xposition + ":" + db.Racks.Where(c => c.RackId == id).Single().Yposition;
                
                List<Shelf> shelves = db.Shelves.Where(c => c.RackId == id).ToList();
                var model = new ShelfModel() { Shelves = shelves, RackId = id };

                return View(model);
            }
        }

        public ActionResult NewShelf()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult NewShelf(Shelf shelf)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                if (ModelState.IsValid)
                {
                    bool isAlreadyRegistered = db.Shelves.Any(c => c.Xposition == shelf.Xposition) && 
                                               db.Shelves.Any(c => c.Yposition == shelf.Yposition) && 
                                               db.Shelves.Any(c => c.RackId == shelf.RackId);
                    if (!isAlreadyRegistered)
                    {
                        shelf.RackId = (int)HttpContext.Session["RackID"];
                        db.Shelves.Add(shelf);
                        db.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("Index", new { iD = Session["RackID"] });
                    }
                    else
                    {
                        TempData["AlreadyExist"] = "You can not create two shelves on the same position";
                    }
                    
                }
                
                return View();
            }
        }

        public ActionResult Create()
        {
            return RedirectToAction("NewShelf");
        }

        public ActionResult Edit(int? id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["ShelfName"] = db.Shelves.Where(c => c.ShelfId == id).Single().Xposition + ":" + db.Shelves.Where(c => c.ShelfId == id).Single().Yposition;
                if (id == null)
                {
                    return new HttpStatusCodeResult(BadRequest);
                }
                Shelf shelf = db.Shelves.Find(id);
                if (shelf == null)
                {
                    return HttpNotFound();
                }

                return View(db.Shelves.SingleOrDefault(c => c.ShelfId == id));
            }
        }

        [HttpPost]
        public ActionResult Edit(Shelf shelf)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                if (ModelState.IsValid)
                {
                    shelf.RackId = (int)HttpContext.Session["RackID"];
                    db.Entry(shelf).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { iD = Session["RackID"] });
                }

                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                Session["ShelfName"] = db.Shelves.Where(c => c.ShelfId == id).Single().Xposition + ":" + db.Shelves.Where(c => c.ShelfId == id).Single().Yposition;
                return View(db.Shelves.SingleOrDefault(c => c.ShelfId == id));
            }
        }

        public ActionResult Delete(int id)
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                var shelf = db.Shelves.SingleOrDefault(c => c.ShelfId == id);
                db.Shelves.Remove(shelf);
                db.SaveChanges();
                return RedirectToAction("Index", new { iD = Session["RackID"] });
            }
        }

        public ActionResult BackToRackIndex()
        {
            return RedirectToAction("Index", "Rack", new {iD = Session["StoreHouseID"]});
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
                                Shelf shelf = new Shelf();
                                shelf.EAN = Int32.Parse(words[0]);
                                shelf.Xposition = Int32.Parse(words[1]);
                                shelf.Yposition = Int32.Parse(words[2]);
                                shelf.Activ = Boolean.Parse(words[3]);
                                shelf.RackId = (int)Session["RackID"];
                                db.Shelves.Add(shelf);
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