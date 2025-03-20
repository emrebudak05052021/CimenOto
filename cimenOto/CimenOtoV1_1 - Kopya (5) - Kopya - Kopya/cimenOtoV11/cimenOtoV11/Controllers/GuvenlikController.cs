using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cimenOtoV11.Models;
using System.Web.Security;
namespace cimenOtoV11.Controllers
{
    public class GuvenlikController : Controller
    {
        db_CimenOtoEntities db = new db_CimenOtoEntities();
        // GET: Guvenlik
        public ActionResult GirisYap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GirisYap(tbl_User t)
        {
            var bilgiler = db.tbl_User.FirstOrDefault(x => x.userName == t.userName && x.password == t.password);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.userName, false);
                Session["UserID"] = bilgiler.kulID;
                Session["Role"] = bilgiler.adminMi;
                if ((byte)Session["Role"] == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("anaSayfa", "Home");
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult cikisYap()
        {
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("GirisYap","Guvenlik");
        }

        [HttpGet]
        public ActionResult KayitOl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KayitOl(tbl_User p)
        {
            var mükerrer = db.tbl_User.FirstOrDefault(x=>x.telNo == p.telNo || x.eMail == p.eMail);
            if(mükerrer != null)
            {
                ViewBag.uyariMesaji = "Girdiğiniz telefon numarası veya E-Mail daha önce kullanılmış!";
                return View();
            }
            else
            {
                if (p != null)
                {
                    if (p.adminMi == null)
                    {
                        p.adminMi = 2;
                    }
                    p.adminMi = null;
                    p.kulID = Guid.NewGuid();
                    db.tbl_User.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("GirisYap", "Guvenlik");
                }
                else
                {
                    return View();
                }
            }
        }
    }
}