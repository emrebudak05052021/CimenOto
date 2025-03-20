using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using cimenOtoV11.Models;
using Iyzipay.Model;
using Iyzipay.Request;
using Iyzipay;
using System.Threading.Tasks;
using cimenOtoV11.yeniSiparisBilgi;
using System.Globalization;
namespace cimenOtoV11.Controllers
{
    public class SiparisController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        public ActionResult siparisFormu()
        {
            // Kullanıcı bilgilerini al
            Guid? user = Session["UserID"] as Guid?;
            var sepet = db.tbl_Sepet.Where(x => x.kulID == user).ToList();
            var userbilgi = db.tbl_User.FirstOrDefault(x => x.kulID == user);
            var adresDetay = db.tbl_Adres.FirstOrDefault(x => x.userID == user);
            // Eğer sepette ürün yoksa, sepet sayfasına yönlendir
            if (!sepet.Any())
            {
                return RedirectToAction("sepetim", "SepetController");
            }

            // Eğer kullanıcı adresi yoksa, adreslerim sayfasına yönlendir
            if (userbilgi.adres == null)
            {
                return RedirectToAction("adreslerim", "User", user);
            }

            // Yeni sipariş oluştur
            var yeniSiparis = new tbl_Siparisler
            {
                kulID = user,
                siparisTarihi = DateTime.Now,
                toplamTutar = sepet.Sum(x => x.toplamTutar),
                sipDurum = 7,  // Sipariş durumu
                adres = userbilgi.adres,
                sozlesmeKabul = false
            };
            db.tbl_Siparisler.Add(yeniSiparis);
            db.SaveChanges();  // Yeni siparişi kaydet


            // Sepetteki ürünleri sipariş detaylarına ekle
            foreach (var item in sepet)
            {
                var siparsiDetay = new tbl_SİparisDetay
                {
                    siparisID = yeniSiparis.siparisID,
                    parcaID = item.parcaID,
                    miktar = item.miktar,
                    birimFiyat = item.birimFiyat,
                    toplamTutar = item.toplamTutar,
                    adres = userbilgi.adres
                };
                db.tbl_SİparisDetay.Add(siparsiDetay);
            }

            // Ürün bilgilerini hazırlamak için sepetteki ürünleri al
            var urunBilgileri = string.Join(", ", sepet.Select(x => x.parcaID).ToArray());  // Ürün adlarını al

            // Sözleşme metnini doldurmak için gerekli parametreleri belirle
            var sozlesmeMetni = @"
MESAFELİ SATIŞ SÖZLEŞMESİ: 
1. TARAFLAR: Bu sözleşme, aşağıda bilgileri bulunan satıcı ile aşağıda bilgileri bulunan alıcı arasında, elektronik ortamda yapılan satış işlemlerini düzenlemek amacıyla oluşturulmuştur.

SATICI: 
Ticaret Unvanı: {0} 
Adres: {1} 
Telefon: {2} 
E-Posta: {3} 
Vergi Dairesi ve No: {4}

ALICI: 
Adı-Soyadı / Ünvanı: {5} 
Adres: {6} 
Telefon: {7} 
E-Posta: {8}

2. KONU: 
İşbu sözleşmenin konusu, ALICI'nın SATICI'ya ait {9} internet sitesinden elektronik ortamda sipariş verdiği, aşağıda belirtilen nitelikleri ve satış fiyatı belirtilen ürünün satışı ve teslimine ilişkin tarafların hak ve yükümlülüklerinin belirlenmesidir.

3. SÖZLEŞME KONUSU ÜRÜN BİLGİLERİ: 
Ürün Adı ve Kodu: {10} 
Miktarı: {11} 
Satış Fiyatı (KDV Dahil): {12} 
Ödeme Şekli: {13} 
Teslimat Adresi: {14} 
Fatura Adresi: {15}

4. TESLİMAT: 
4.1. Ürünler, sipariş onaylandıktan sonra {16} gün içinde kargoya verilecektir. 
4.2. Teslimat, ALICI'nın belirttiği adresine yapılacaktır. 
4.3. Kargo ücreti {17} olup, ödeme sırasında SATICI tarafından karşılanacaktır.

5. CAYMA HAKKI: 
5.1. ALICI, ürünü teslim aldığı tarihten itibaren 14 gün içinde herhangi bir gerekçe göstermeksizin cayma hakkına sahiptir. 
5.2. Cayma hakkı kapsamında, ürünün kullanılmamış ve tekrar satılabilir durumda olması gerekmektedir. 
5.3. Cayma hakkının kullanılması durumunda, iade kargo ücreti {18} tarafından karşılanacaktır.

6. GARANTİ VE İADE KOŞULLARI: 
6.1. Satıcı, satılan ürünlerin ayıplı olması durumunda Tüketicinin Korunması Hakkında Kanun kapsamında gerekli işlemleri yapacaktır. 
6.2. ALICI, ayıplı ürünleri teslim aldıktan sonra 1 gün içinde SATICI'ya bildirimde bulunmalıdır.

7. GİZLİLİK: 
Taraflar, bu sözleşme kapsamında edindikleri kişisel verileri 6698 Sayılı Kişisel Verilerin Korunması Kanunu çerçevesinde saklayacağını ve üçüncü kişilerle paylaşmayacağını taahhüt eder.

8. YETKİLİ MAHKEME: 
İşbu sözleşmenin uygulanmasında, Tüketici Hakem Heyetleri ve Tüketici Mahkemeleri yetkilidir. 
İşbu sözleşme, elektronik ortamda onaylanarak yürürlüğe girmiştir.

SATICI: {0} 
ALICI: {5} 
Tarih: {19}
";

            // Parametreleri doldur
            var sozlesmeMetniDoldurulmus = string.Format(sozlesmeMetni,
                "Çimen Oto Yedek Parça",              // {0} SATICI
                "Adnan Menderes Mahallesi Parçacılar Sokak No 48/A Merkez/Osmaniye", // {1}
                "+90 507 015 0480",                   // {2}
                "cimenoto80@gmail.com",               // {3}
                "Osmaniye / 2111503878",              // {4}
                userbilgi.ad + " " + userbilgi.soyad,                          // {5} ALICI
                userbilgi.adres,                       // {6}
                userbilgi.telNo,                     // {7}
                userbilgi.eMail,                      // {8}
                "https://www.guarddevelop.com.tr",    // {9}
                urunBilgileri,                       // {10} Ürün bilgileri (sepetteki ürünler)
                sepet.Sum(x => x.miktar),            // {11} Toplam miktar
                sepet.Sum(x => x.toplamTutar),       // {12} Toplam tutar
                "Ödeme Şekli",                        // {13}
                userbilgi.adres,                     // {14} Teslimat adresi
                userbilgi.adres,                     // {15} Fatura adresi
                5,                                    // {16} Teslimat süresi
                "Ücret Bilgisi",                     // {17} Kargo ücreti
                "Alıcı",                              // {18} İade durumu
                DateTime.Now.ToString("dd/MM/yyyy")  // {19} Tarih
            );

            // Sözleşmeyi kaydet
            var mesafeli = new MesafeliSatisSozlesmeleri
            {
                MusteriID = user,
                SiparisID = yeniSiparis.siparisID,
                SozlesmeMetni = sozlesmeMetniDoldurulmus,
                OlusturmaTarihi = DateTime.Now
            };

            db.MesafeliSatisSozlesmeleri.Add(mesafeli);
            db.SaveChanges();

            // Ödeme sayfasına yönlendir
            return RedirectToAction("OdemeFormu");
        }

        public ActionResult OdemeFormu()
        {
            // Kullanıcıya ödeme formunda sipariş bilgilerini göstermek isterseniz sipariş bilgisini model olarak geçebilirsiniz.
            Guid? kullaniciID = Session["UserID"] as Guid?;
            var siparis = db.tbl_Siparisler
                .Where(x => x.kulID == kullaniciID)
                .OrderByDescending(x => x.siparisTarihi)
                .FirstOrDefault();
            // Ödeme formunu, boş ödeme modeliyle (kart bilgileri kullanıcı tarafından girilecek) çağırıyoruz.
            return View(new OdemeModeli { Tutar = siparis.toplamTutar ?? 0 });
        }
        private string GetUserIp()
        {
            string ip = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ip))
            {
                string[] ipList = ip.Split(',');
                return ipList[0]; // Eğer birden fazla proxy varsa, ilk gerçek IP'yi al
            }

            return HttpContext.Request.ServerVariables["REMOTE_ADDR"];
        }
        [HttpPost]
        public async Task<ActionResult> OdemeIslemi(OdemeModeli odemeModeli)
        {
            // Kullanıcı bilgilerini al
            Guid? kullaniciID = Session["UserID"] as Guid?;
            var sepet = db.tbl_Sepet.Where(x => x.kulID == kullaniciID).ToList();
            var userbilgi = db.tbl_User.FirstOrDefault(x => x.kulID == kullaniciID);
            var adresDetay = db.tbl_Adres.FirstOrDefault(x => x.userID == kullaniciID);
            // Kullanıcının son siparişini alıyoruz
            var siparis = db.tbl_Siparisler
                .Where(x => x.kulID == kullaniciID)
                .OrderByDescending(x => x.siparisTarihi)
                .FirstOrDefault();
            if (siparis == null)
            {
                return RedirectToAction("siparisFormu");
            }

            if (!odemeModeli.SozlesmeKabul)
            {
                ViewBag.HataMesaji = "Ödeme işlemini tamamlanadan önce 'Mesafeli Satış Sözleşmesi' ni onaylamanız gerekmektedir.";
                return View("OdemeFormu", odemeModeli);
            }

            // İyzico API ayarları (sandbox ortamı)
            var secenekler = new Options();
            secenekler.ApiKey = "sandbox-hFM0WdVQ5Q7AQbIOpdcsSYYXUyiTOQsk";
            secenekler.SecretKey = "sandbox-nmxSFEkB0szr4YfjusMXwKemUQL3e5P2";
            secenekler.BaseUrl = "https://sandbox-api.iyzipay.com";
            var kontrol = db.tbl_Siparisler.Where(x => x.kulID == kullaniciID).OrderByDescending(x => x.siparisTarihi).FirstOrDefault();
            // Sipariş varsa ve zaten ödendiyse tekrar ödeme işlemini başlatma
            if (kontrol != null && siparis.sipDurum == 1)
            {
                ViewBag.HataMesaji = "Bu sipariş için zaten ödeme yapılmıştır!";
                return View("OdemeBasarisiz");
            }
            // Ödeme isteğini hazırlıyoruz
            var odemeIstegi = new CreatePaymentRequest();
            odemeIstegi.Locale = Locale.TR.ToString();
            odemeIstegi.ConversationId = siparis.siparisID.ToString();

            decimal tutar = siparis.toplamTutar ?? 0m; // Null kontrolünü daha kısa ve güvenli hale getirdik.

            odemeIstegi.Price = tutar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            odemeIstegi.PaidPrice = tutar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            odemeIstegi.Currency = Currency.TRY.ToString();
            odemeIstegi.Installment = 1;
            odemeIstegi.BasketId = siparis.siparisID.ToString();
            odemeIstegi.PaymentChannel = PaymentChannel.WEB.ToString();
            odemeIstegi.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            // Kullanıcıdan alınan kart bilgilerini ekliyoruz
            PaymentCard odemeKarti = new PaymentCard();
            odemeKarti.CardHolderName = odemeModeli.KartSahibiAdi;
            odemeKarti.CardNumber = odemeModeli.KartNumarasi;
            odemeKarti.ExpireMonth = odemeModeli.SonKullanmaAyi;
            odemeKarti.ExpireYear = odemeModeli.SonKullanmaYili;
            odemeKarti.Cvc = odemeModeli.GuvenlikKodu;
            odemeKarti.RegisterCard = 0;
            odemeIstegi.PaymentCard = odemeKarti;

            // Alıcı bilgilerini tbl_User tablosundan alıyoruz
            var kullaniciBilgi = db.tbl_User.FirstOrDefault(x => x.kulID == kullaniciID);
            Buyer buyer = new Buyer();
            buyer.Id = userbilgi.kulID.ToString(); // Alıcının ID'si
            buyer.Name = userbilgi.ad; // Alıcının adı
            buyer.Surname = userbilgi.soyad; // Alıcının soyadı
            buyer.GsmNumber = userbilgi.telNo; // Alıcının telefon numarası
            buyer.Email = userbilgi.eMail; // Alıcının e-posta adresi
            buyer.IdentityNumber = odemeModeli.tc; // Alıcının kimlik numarası
            //buyer.LastLoginDate = "2023-01-01 12:00:00"; // Alıcının son giriş tarihi
            //buyer.RegistrationDate = "2022-01-01 12:00:00"; // Alıcının kayıt tarihi
            buyer.RegistrationAddress = adresDetay.adresSatir1 + "| " + adresDetay.adresSatir2; // Alıcının adresi
            buyer.Ip = GetUserIp(); // Alıcının IP adresi
            buyer.City = adresDetay.il; // Alıcının şehri
            buyer.Country = "Turkey"; // Alıcının ülkesi
            buyer.ZipCode = adresDetay.postaKodu; // Alıcının posta kodu
            odemeIstegi.Buyer = buyer;

            // Kargo adresi bilgileri (örnek)
            Address kargoAdresi = new Address();
            kargoAdresi.ContactName = kullaniciBilgi.userName;
            kargoAdresi.City = adresDetay.il;
            kargoAdresi.Country = "Turkey";
            kargoAdresi.Description = adresDetay.adresSatir1 + "| " + adresDetay.adresSatir2;
            kargoAdresi.ZipCode = adresDetay.postaKodu;
            odemeIstegi.ShippingAddress = kargoAdresi;

            // Fatura adresi bilgileri (örnek)
            Address faturaAdresi = new Address();
            faturaAdresi.ContactName = kullaniciBilgi.userName;
            faturaAdresi.City = adresDetay.il;
            faturaAdresi.Country = "Turkey";
            faturaAdresi.Description = adresDetay.adresSatir1 + "| " + adresDetay.adresSatir2;
            faturaAdresi.ZipCode = adresDetay.postaKodu;
            odemeIstegi.BillingAddress = faturaAdresi;

            // Sepet öğesi bilgisi (örneğin sipariş ödemesi)
            List<BasketItem> sepetOgesiListesi = new List<BasketItem>();
            BasketItem sepetOgesi = new BasketItem();
            sepetOgesi.Id = siparis.siparisID.ToString();
            sepetOgesi.Name = "Sipariş Ödemesi";
            sepetOgesi.Category1 = "Ürün";
            sepetOgesi.Category2 = "Ödeme";
            sepetOgesi.ItemType = BasketItemType.PHYSICAL.ToString();
            sepetOgesi.Price = tutar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            sepetOgesiListesi.Add(sepetOgesi);
            odemeIstegi.BasketItems = sepetOgesiListesi;

            // İyzico API çağrısı: Ödeme işlemini asenkron olarak gerçekleştiriyoruz
            Payment odemeSonucu = await Payment.Create(odemeIstegi, secenekler);

            // Ödeme başarılı ise
            if (odemeSonucu.Status == "success")
            {
                // Sipariş durumunu güncelle: sipDurum = 1 (Ödeme başarılı)
                siparis.sipDurum = 1;
                siparis.sozlesmeKabul = true;
                db.SaveChanges();
                // Ödeme başarılı view'ı: "OdemeBasarili" adlı view içerisinde kullanıcıya bilgilendirme yapılır.

                // ✅ Sipariş alındığında e-posta gönder
                string aliciEmail = "emrebudak05052021@gmail.com"; // Bildirim e-postası alacak adres
                string konu = "Çimen Oto Web Yeni Sipariş Alındı.";
                string icerik = $@"
        <h2>Çimen Oto Web Yeni Sipariş Alındı.</h2>
<p><strong>Ad Soyad:</strong> {userbilgi.ad + " " + userbilgi.soyad}</p>
        <p><strong>Sipariş Numarası:</strong> {siparis.siparisID}</p>
        <p><strong>Toplam Tutar:</strong> {siparis.toplamTutar} TL</p>
        <p><strong>Adres:</strong> {adresDetay.adresSatir1 + " | " + adresDetay.il + " | " + adresDetay.ilce}</p>
        <p><strong>Telefon:</strong> {userbilgi.telNo}</p>
    ";

                MailHelper.SendEmail(aliciEmail, konu, icerik);
                // Sepeti boşalt
                db.tbl_Sepet.RemoveRange(sepet);
                db.SaveChanges();

                return View("OdemeBasarili", odemeSonucu);


            }
            else
            {
                // Ödeme başarısız ise hata mesajını view'da gösterelim
                ViewBag.HataMesaji = odemeSonucu.ErrorMessage;
                return View("OdemeBasarisiz", odemeSonucu);
            }
        }

        public ActionResult siparislerim(Guid? id)
        {
            var siparis = db.tbl_Siparisler.Where(x => x.kulID == id).ToList();
            return View(siparis);
        }
        public ActionResult siparislerimDetay(int? id)
        {
            var sipDetay = db.tbl_SİparisDetay.Where(x => x.siparisID == id).ToList();
            return View(sipDetay);

        }
    }
}