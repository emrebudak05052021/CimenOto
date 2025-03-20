using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cimenOtoV11.Models;
namespace cimenOtoV11.Controllers
{
    public class SiparisDurumController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: SiparisDurum
        public ActionResult sdIndex()
        {
            var list = db.tbl_SiparisDurum.ToList();
            return View(list);
        }
        public ActionResult durumGetir(int? id)
        {
            var getir = db.tbl_SiparisDurum.FirstOrDefault(x => x.durumID == id);
            return View(getir);
        }
        public ActionResult guncelle(tbl_SiparisDurum p)
        {
            var bul = db.tbl_SiparisDurum.Find(p.durumID);
            bul.durum = p.durum;
            db.SaveChanges();
            return RedirectToAction("sdIndex");
        }
        [HttpGet]
        public ActionResult durumEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult durumEkle(tbl_SiparisDurum p)
        {
            db.tbl_SiparisDurum.Add(p);
            db.SaveChanges();
            return RedirectToAction("sdIndex");
        }

        public ActionResult sil(int? id)
        {
            var silinen = db.tbl_SiparisDurum.FirstOrDefault(x=>x.durumID==id);
            db.tbl_SiparisDurum.Remove(silinen);
            db.SaveChanges();
            return RedirectToAction("sdIndex");
        }
    }
}