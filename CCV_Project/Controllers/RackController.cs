using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCV_Project.Models;

namespace CCV_Project.Controllers
{
    public class RackController : Controller
    {
        //private int StoreHouseID = ViewDataInfo;
        // GET: Rack


        //Ehm čau Paťo, potreboval by som poradiť ... ak sa dívaš na tento kod asi si sa rozhodol že mi poradíš takže tu je môj problém
        //potrebujem dostať "Id" zo StoreHouseContorller sem do RackControler
        public ActionResult Index(int id) // ---> tu sa mi to čiastočne podarilo a tak môžem vypísať Racks ktoré patria do StoreHouse na ktorý si klikol, konkrétne to robím cez redirect to action s parametrom
        {
            using (CCV_Tables_Context db = new CCV_Tables_Context())
            {
                return View(db.Racks.ToList().Where(c=>c.StoreHouseRefId == id));
            }
        }
        //avšak, ked chcem vytvoriť nový rack ktorý patrí do daného StoreHouse (viz CCV_Tables - Foreign key) tak narážam na problém ako určiť aké ID má storeHouse v ktorom sa práve nachádzam
        //skúšal som všeličo ... najlepšie asi fungovali TempData ale ked refreshnem stránku alebo nejdem priamo zo storeHouse, robí to kktiny
        //napadlo ma riešiť to tak že si budem ID ukladať do databázy a budem si ho udržiavať po celý čas čo budem v danom StoreHouse ale príde mi to príliš komplikované, určite existuje niečo jednoduchšie...
        //ak ťa niečo napadlo daj vedieť ... kludne to dopíš do kodu a pushni späť ...inak asi si si všimol že používam všade using (CCV_Tables_Context db = new CCV_Tables_Context())
        //popravde newm či je to dobre takto alebo mi stačí spraviť niečo ako private CCV_Tables_Context _db = new CCV_Tables_Context() hned na začiatku kodu a požívať všade len _db
        //daj vedieť čo je lepšie ... príde mi že kým sa aplikácia spustí trvá to celú večnosť ...
        // toť asi vše .. teda aspoň k tomuto projektu ... žiadny indický c# youtuber ani stackoverflow nepomohli tak snád budeš vedieť ty ... :D

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
                    db.Racks.Add(rack);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return View();
            }
        }

        public ActionResult Create()
        {
            return RedirectToAction("NewRack");
        }
    }
}