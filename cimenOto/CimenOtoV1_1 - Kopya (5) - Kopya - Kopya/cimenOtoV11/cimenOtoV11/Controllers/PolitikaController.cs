using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cimenOtoV11.Models;
namespace cimenOtoV11.Controllers
{
    public class PolitikaController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: Politika
        public ActionResult teslimaVeIade()
        {
            return View();
        }
        public ActionResult gizlilikPolitikasi()
        {
            return View();
        }
        public ActionResult urunlerHakkinda()
        {
            return View();
        }

        public ActionResult mesafeliSatisSozlesmesi()
        {
            return View();
        }
    }
}