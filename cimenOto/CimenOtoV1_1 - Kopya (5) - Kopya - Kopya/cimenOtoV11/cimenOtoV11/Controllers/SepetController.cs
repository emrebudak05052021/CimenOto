using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cimenOtoV11.Models;
namespace cimenOtoV11.Controllers
{
    public class SepetController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: Sepet
        [Authorize]
        public ActionResult Sepetim()
        {
            Guid? userID = Session["UserID"] as Guid?;
            var sepetim = db.tbl_Sepet.Where(x=>x.kulID == userID).ToList();
            return View(sepetim);
        }

        public ActionResult sepetUrunSil(int? id)
        {
            var silinen = db.tbl_Sepet.FirstOrDefault(x=>x.sepetID == id);
            if (silinen != null)
            {
                db.tbl_Sepet.Remove(silinen);
                db.SaveChanges();
            }
            return RedirectToAction("Sepetim","Sepet");
            
        }      
    }
}