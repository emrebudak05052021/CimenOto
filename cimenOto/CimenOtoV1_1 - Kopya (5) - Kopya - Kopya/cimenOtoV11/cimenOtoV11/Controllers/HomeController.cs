using cimenOtoV11.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace cimenOtoV11.Controllers
{
    public class HomeController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        public ActionResult anaSayfa()
        {
            return View();
        }
        public ActionResult kartListele(int page = 1, int pageSize = 12)
        {
            //var liste = db.tbl_AracParca.ToList();
            //return View(liste);
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

        [HttpGet]
        public ActionResult parcaAra()
        {
            var markaList = (from i in db.tbl_AracMarka.ToList()
                             select new SelectListItem
                             {
                                 Text = i.markaAdi,
                                 Value = i.markaID.ToString()
                             }
             ).ToList();
            ViewBag.mrk = markaList;
            return View();
        }
        [HttpPost]
        public ActionResult parcaAra(int? markaID, string model, string orginalNo,string pad)
        {
            var markaList = (from i in db.tbl_AracMarka.ToList()
                             select new SelectListItem
                             {
                                 Text = i.markaAdi,
                                 Value = i.markaID.ToString()
                             }
             ).ToList();
            ViewBag.mrk = markaList;


            var filtre = db.tbl_AracParca.Where(x => x.markaID == markaID && (string.IsNullOrEmpty(model) || x.modelID.Contains(model)) && (string.IsNullOrEmpty(orginalNo) || x.orginalKod.Contains(orginalNo)) && (string.IsNullOrEmpty(pad) || x.pAd.Contains(pad))).ToList();
    
            
    

            return View(filtre);

        }
        public ActionResult hakkimizda()
        {
            return View();
        }
        public ActionResult bizeUlasin()
        {
            return View();
        }
        public ActionResult magazamiz()
        {
            return View();
        }
        public ActionResult misyonVizyon()
        {
            return View();
        }
    }
}