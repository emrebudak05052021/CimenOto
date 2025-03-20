using cimenOtoV11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace cimenOtoV11.Controllers
{
    public class UrunDetayController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: UrunDetay
        public ActionResult UrunDetayListele(int? id)
        {
            var degerler = db.tbl_AracParca.FirstOrDefault(x => x.parcaID == id);
            return View(degerler);
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult sepeteEkle(tbl_AracParca p, byte adet)
        {
            try
            {
                Guid? user = Session["UserID"] as Guid?;
                if(user != null)
                {
                    // Toplam tutarı hesapla
                    decimal? toplamTutar = p.kdvMaliyet * adet;
                    decimal? birimFiyat = p.kdvMaliyet * 1;
                    // Yeni sepet öğesi oluştur
                    tbl_Sepet spt = new tbl_Sepet
                    {
                        kulID = user,
                        parcaID = p.parcaID,
                        miktar = adet,
                        birimFiyat = birimFiyat,
                        toplamTutar = toplamTutar,
                        eklenmeTarihi = DateTime.Now
                    };

                    // Veritabanına ekle ve değişiklikleri kaydet
                    db.tbl_Sepet.Add(spt);
                    db.SaveChanges();

                    // Başarılı olduğunda sepet sayfasına yönlendirme
                    return RedirectToAction("Sepetim", "Sepet");
                }
                else
                {
                    return RedirectToAction("GirisYap","Guvenlik");
                }

            }
            catch (Exception ex)
            {
                // Hata durumunu işleyin
                Console.WriteLine(ex.Message);
                // Hata sayfasına yönlendirme veya uygun bir işlem yapma
                return RedirectToAction("HataSayfasi", "Hata");
            }
        }
    }
}