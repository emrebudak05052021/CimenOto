using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages.Html;
using cimenOtoV11.Models;
namespace cimenOtoV11.Controllers
{
    public class UserController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: User

        //*************ADRES*********************
        [Authorize]
        public ActionResult adreslerim()
        {
            Guid? user = Session["UserID"] as Guid?;
            var adrs = db.tbl_Adres.Where(x=>x.userID == user).ToList();
            return View(adrs);
        }
        [Authorize]
        [HttpGet]
        public ActionResult adresEkle()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult adresEkle(tbl_Adres p)
        {
            if(p != null)
            {
                Guid? user = Session["UserID"]as Guid?;
                p.olusturmaTarih = DateTime.Now;
                p.userID = user;
                db.tbl_Adres.Add(p);
                db.SaveChanges();
                return RedirectToAction("adreslerim");
            }

            return View();
        }
        [Authorize]
        public ActionResult adresGetir(int? id)
        {
            var gelenAdres = db.tbl_Adres.FirstOrDefault(x=>x.adresID == id);
            return View(gelenAdres);
        }
        [Authorize]
        public ActionResult Guncelle(tbl_Adres p)
        {
            if (p == null)
            {
                // Gelen model null ise hata mesajı döndürün
                return Content("Gönderilen model null. Form alanlarını kontrol edin.");
            }
            var adrs = db.tbl_Adres.Find(p.adresID);
            adrs.adresSatir1 = p.adresSatir1;
            adrs.adresSatir2 = p.adresSatir2;
            adrs.il = p.il;
            adrs.ilce = p.ilce;
            adrs.postaKodu = p.postaKodu;
            adrs.adresTuru = p.adresTuru;
            db.SaveChanges();
            return RedirectToAction("adreslerim");
        }

        //*************HESABIM*********************
        [Authorize]
        public ActionResult hesapGetir(Guid? id)
        {
            var degerler = (from i in db.tbl_Adres.Where(x => x.userID == id)
                            select new System.Web.Mvc.SelectListItem()
                            {
                                Value = i.adresID.ToString(), 
                                Text = i.adresSatir1         
                            }).ToList();
            ViewBag.dgr = degerler;
            var hsp = db.tbl_User.FirstOrDefault(x=>x.kulID == id);
            return View(hsp);
        }
        [Authorize]
        public ActionResult hesapGuncelle(tbl_User p)
        {
            var userBul = db.tbl_User.Find(p.kulID);
            userBul.kulID = p.kulID;
            userBul.userName = p.userName;
            userBul.password = p.password;
            userBul.eMail = p.eMail;
            userBul.telNo = p.telNo;
            userBul.adres = p.adres;
            db.SaveChanges();
            return RedirectToAction("anaSayfa","Home");
        }
    }
}