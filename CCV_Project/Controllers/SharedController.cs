using System.Web.Mvc;

namespace CCV_Project.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult SingOut()
        {
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Create()
        {
            return RedirectToAction("Create");
        }
    }
}