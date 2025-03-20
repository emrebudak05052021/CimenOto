using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using cimenOtoV11.Models;
namespace cimenOtoV11.Controllers
{
    public class MarkaModelParcalarController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: MarkaModelParcalar
        public ActionResult Index(short? id, int page = 1, int pageSize = 12)
        {
            //var liste = db.tbl_AracParca.Where(x=>x.markaID == id).ToList();
            //return View(liste);
            //var liste = db.tbl_AracParca.ToList();
            //return View(liste);
            var totalRecords = db.tbl_AracParca.Count();  // Toplam kayıt sayısı
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);  // Toplam sayfa sayısı

            var model = db.tbl_AracParca
                .Where(x => x.markaID == id)
                          .OrderBy(p => p.parcaID)  // Gerekirse sıralama ekleyebilirsiniz
            .Skip((page - 1) * pageSize)  // Kaçıncı sayfa olduğu
            .Take(pageSize)  // Her sayfada gösterilecek kayıt sayısı
                          .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(model);
        }

        public ActionResult Parcalar(string id)
        {
            var liste = db.tbl_AracParca.Where(x => x.modelID == id).ToList();
            return View(liste);
        }
    }
}