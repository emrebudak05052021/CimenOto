using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.WebPages;
using cimenOtoV11.Models;
namespace cimenOtoV11.Controllers
{
    public class adminParcaController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: adminParca
        public ActionResult Index(int page = 1, int pageSize = 20)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var totalRecords = db.tbl_AracParca.Count();  // Toplam kayıt sayısı
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);  // Toplam sayfa sayısı

                var model = db.tbl_AracParca
                              .OrderBy(p => p.parcaID)  // Gerekirse sıralama ekleyebilirsiniz
                .Skip((page - 1) * pageSize)
                .Take(pageSize)  // Her sayfada gösterilecek kayıt sayısı
                              .ToList();

                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = page;

                return View(model);
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }

        [HttpGet]
        public ActionResult parcaAra()
        {
            return View();
        }
        [HttpPost]
        public ActionResult parcaAra(string islem, string aracModel, string ureticiKod, string orginalNo, string parcaAdi, string markaID, int page = 1, int pageSize = 20)
        {
            var hataMesajlari = new List<string>();
            if (islem == "model_parca")
            {
                if (string.IsNullOrEmpty(aracModel))
                {
                    hataMesajlari.Add("Model alanı boş bırakılamaz.");
                }
                if (string.IsNullOrEmpty(parcaAdi))
                {
                    hataMesajlari.Add("Parça İsmi alanı boş bırakılamaz.");
                }

                if (hataMesajlari.Any())
                {
                    ViewBag.HataMesajlari = hataMesajlari;
                    return View();
                }

                var list = db.tbl_AracParca.Where(x => x.pAd.Contains(parcaAdi) && x.modelID.Contains(aracModel)).ToList();
                return View(list);
            }

            if (islem == "üretici_orginal")
            {
                if (string.IsNullOrEmpty(ureticiKod))
                {
                    hataMesajlari.Add("Üretici Kod alanı boş bırakılamaz.");
                }
                if (string.IsNullOrEmpty(orginalNo))
                {
                    hataMesajlari.Add("Orjinal No alanı boş bırakılamaz.");
                }
                if (hataMesajlari.Any())
                {
                    ViewBag.HataMesajlari = hataMesajlari;
                    return View();
                }
                var list2 = db.tbl_AracParca.Where(x => x.orginalKod.Contains(orginalNo) || x.ureticiKod.Contains(ureticiKod)).ToList();
                return View(list2);
            }
            if (islem == "orginalNo")
            {
                if (string.IsNullOrEmpty(orginalNo))
                {
                    hataMesajlari.Add("Orjinal No alanı boş bırakılamaz!");
                }
                if (hataMesajlari.Any())
                {
                    ViewBag.HataMesajlari = hataMesajlari;
                    return View();
                }
                var list2 = db.tbl_AracParca.Where(x => x.orginalKod.Contains(orginalNo)).ToList();
                return View(list2);
            }
            if (islem == "ureticiKod")
            {
                if (string.IsNullOrEmpty(ureticiKod))
                {
                    hataMesajlari.Add("Üretici Kod alanı boş bırakılamaz.");
                }
                if (hataMesajlari.Any())
                {
                    ViewBag.HataMesajlari = hataMesajlari;
                    return View();
                }
                var list2 = db.tbl_AracParca.Where(x => x.ureticiKod.Contains(ureticiKod)).ToList();
                return View(list2);
            }

            if(islem =="markaID")
            {
                if(string.IsNullOrEmpty(markaID))
                {
                    hataMesajlari.Add("Marka ID alanı boş bırakılamaz.");
                }
                if(hataMesajlari.Any())
                {
                    ViewBag.HataMesajlari = hataMesajlari;
                    return View();
                }
                int markaIdDonusum = Convert.ToInt32(markaID);
                var list = db.tbl_AracParca.Where(x => x.markaID == markaIdDonusum).ToList();
                return View(list);
            }
            return View();
        }

        [HttpGet]
        public ActionResult parcaEkle()
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var degerler = db.tbl_AracMarka
                .OrderBy(x => x.markaAdi)
                .Select(i => new SelectListItem
                {
                    Text = i.markaAdi,
                    Value = i.markaID.ToString()
                }).ToList();

                ViewBag.dgr = degerler;
                return View();
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }

        [HttpPost]
        public ActionResult parcaEkle(tbl_AracParca model)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                if (ModelState.IsValid)
                {
                    db.tbl_AracParca.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                // Eğer hata varsa, tekrar View'e dön ve hataları göster
                var degerler = db.tbl_AracMarka.OrderBy(x => x.markaAdi)

                    .Select(i => new SelectListItem
                    {
                        Text = i.markaAdi,
                        Value = i.markaID.ToString()
                    }).ToList();

                ViewBag.dgr = degerler;
                return View(model);
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        public ActionResult sil(int? id)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var silinen = db.tbl_AracParca.FirstOrDefault(x => x.parcaID == id);
                db.tbl_AracParca.Remove(silinen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }

        public ActionResult parcaGetir(int? id)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var lst = db.tbl_AracParca.FirstOrDefault(x => x.parcaID == id);
                return View(lst);
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }

        public ActionResult guncelle(tbl_AracParca p)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var prc = db.tbl_AracParca.Find(p.parcaID);
                prc.yil = p.yil;
                prc.pAd = p.pAd;
                prc.modelID = p.modelID;
                prc.Uretici = p.Uretici;
                prc.ureticiKod = p.ureticiKod;
                prc.orginalKod = p.orginalKod;
                prc.aciklama = p.aciklama;
                prc.icerik = p.icerik;
                prc.motorTip = p.motorTip;
                prc.isk = p.isk;
                prc.kdv = p.kdv;
                prc.listeFiyat = p.listeFiyat;
                prc.kdvMaliyet = p.kdvMaliyet;
                prc.stokMiktari = p.stokMiktari;
                if (prc != null)
                {
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }

        public ActionResult eksikFotoğraf()
        {
            var eksikfotoğraflar = new List<int>();
            var parcalar = db.tbl_AracParca.ToList();

            foreach (var item in parcalar)
            {
                // Fotoğraf yolunu kontrol et
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.parcaImage.TrimStart('/'));
                if (!System.IO.File.Exists(filePath))
                {
                    eksikfotoğraflar.Add(item.parcaID); // Eksik fotoğrafı olan ürün ID'sini listeye ekle
                }
            }
            // Eksik fotoğrafı olan ürünleri bir View'e gönderebilirsiniz
            return View(eksikfotoğraflar);
        }
    }
}