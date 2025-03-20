using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cimenOtoV11.Models;
namespace cimenOtoV11.Controllers
{
    public class adminMarkaController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: adminMarka
        [Authorize]
        public ActionResult Index()
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var list = db.tbl_AracMarka.OrderBy(x => x.markaID).ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        //MARKA SİL
        public ActionResult sil(int? id)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var silinen = db.tbl_AracMarka.Find(id);
                if (silinen != null)
                {
                    try
                    {
                        db.tbl_AracMarka.Remove(silinen);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {

                        ViewBag.Error = "Bu kayıt başka tablolarla ilişkili olduğu için silinemiyor.";
                        return View("Error");
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
            
        }
        //MARKA EKLE
        [HttpGet]
        public ActionResult ekle()
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                return View();
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        [HttpPost]
        public ActionResult ekle(FormCollection m)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                tbl_AracMarka mrk = new tbl_AracMarka();
                string girilenMarka = m["vmarkaadi"].ToString();
                var kontrol = db.tbl_AracMarka.FirstOrDefault(x => x.markaAdi == girilenMarka);
                if (kontrol != null)
                {
                    return View();
                }
                else if (m != null)
                {
                    mrk.markaAdi = m["vmarkaadi"];
                    db.tbl_AracMarka.Add(mrk);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }
        public ActionResult markaGetir(int? id)
        {
            var gelenMarka = db.tbl_AracMarka.Find(id);
            return View(gelenMarka);
        }
        public ActionResult guncelle(tbl_AracMarka p)
        {
            Guid? user = Session["UserID"] as Guid?;
            if (Session["UserID"]?.ToString() == "f5651208-7da0-495e-a481-ea152a65c46e")
            {
                var mrk = db.tbl_AracMarka.Find(p.markaID);
                mrk.markaAdi = p.markaAdi;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("kartListele", "Home");
            }
        }

    }
}