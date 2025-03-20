using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using cimenOtoV11.Models;
namespace cimenOtoV11.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        db_CimenOtoEntities db = new db_CimenOtoEntities();

        [Authorize]

        public ActionResult Index()
        {
            Guid? user = Session["UserID"] as Guid?;
            var siparis = db.tbl_Siparisler.OrderBy(x => x.sipDurum).ToList();
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                return View(siparis);
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult uyeIslemleri()
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var list = db.tbl_User.ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult uyeIslemleri(string tel)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var list = db.tbl_User.Where(x => x.telNo == tel).ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        [Authorize]
        public ActionResult uyeGetir(Guid? id)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var detay = db.tbl_User.FirstOrDefault(x => x.kulID == id);
                return View(detay);
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        [Authorize]
        public ActionResult uyeGuncelle(tbl_User p)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var uye = db.tbl_User.Find(p.kulID);
                uye.password = p.password;
                uye.eMail = p.eMail;
                uye.telNo = p.telNo;
                db.SaveChanges();
                return RedirectToAction("uyeIslemleri");
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        public ActionResult siparisDetay(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Siparis");
            }

            var siparisDetay = db.tbl_SİparisDetay.FirstOrDefault(x => x.siparisID == id);
            if (siparisDetay == null)
            {
                return HttpNotFound();
            }

            var siparis = db.tbl_Siparisler.FirstOrDefault(x => x.siparisID == id);
            var musteri = db.tbl_User.FirstOrDefault(x => x.kulID == siparis.kulID);
            var parca = db.tbl_AracParca.FirstOrDefault(x => x.parcaID == siparisDetay.parcaID);
            var parcalar = db.tbl_AracParca
                 .Where(x => db.tbl_SİparisDetay
                                .Where(sd => sd.siparisID == id)
                                .Select(sd => sd.parcaID)
                                .Contains(x.parcaID))
                 .ToList();
            var viewModel = new SiparisDetayViewModel
            {
                SiparisDetay = siparisDetay,
                Siparis = siparis,
                Musteri = musteri,
                Parca = parca,
                Parcalar = parcalar // Listeyi ekledik
            };

            return View(viewModel);
        }


        public ActionResult siparisDurum(int? id)
        {
            var lst = db.tbl_Siparisler.FirstOrDefault(x => x.siparisID == id);
            if (lst == null)
            {
                return HttpNotFound();
            }

            // Durumları dropdown için ViewBag.dgr'ye ekle
            var durum = (from i in db.tbl_SiparisDurum.ToList()
                         select new SelectListItem
                         {
                             Text = i.durum,
                             Value = i.durumID.ToString()
                         }).ToList();
            ViewBag.dgr = durum;

            return View(lst); // Tek bir sipariş nesnesi gönderiyoruz
        }

        [HttpPost]
        public ActionResult DurumGuncelle(tbl_Siparisler p)
        {
            var durum = (from i in db.tbl_SiparisDurum.ToList()
                         select new SelectListItem
                         {
                             Text = i.durum,
                             Value = i.durumID.ToString()
                         }).ToList();
            ViewBag.dgr = durum;

            // Veritabanındaki siparişi bul
            var sp = db.tbl_Siparisler.FirstOrDefault(s => s.siparisID == p.siparisID);

            if (sp != null)
            {
                // Siparişin durumunu güncelle
                sp.sipDurum = p.sipDurum;

                // Veritabanına kaydet
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}