using E_Ticaret.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret.Controllers
{
    public class UrunController : Controller
    {
        // GET: Urun

        Entities1 db = new Entities1();
        [Authorize]
        public ActionResult Index()
        {
            var list = db.Urun.ToList();
            return View(list);
        }
        //[Authorize (Roles ="A")]
        public ActionResult Ekle()
        {
            List<SelectListItem> deger1 = (from x in db.Kategori.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Ad,
                                               Value = x.Id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View();
        }
        //[Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Ekle(Urun Data,HttpPostedFileBase File)
        {
            string path = Path.Combine("~/Content/Image" + File.FileName);
            File.SaveAs(Server.MapPath(path));
            Data.Resim = File.FileName.ToString();
            db.Urun.Add(Data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       // [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            var urun = db.Urun.Where(x => x.ID == id).FirstOrDefault();
            db.Urun.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
       // [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            var guncelle= db.Urun.Where(x => x.ID == id).FirstOrDefault();
            List<SelectListItem> deger1 = (from x in db.Kategori.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Ad,
                                               Value = x.Id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
           
            return View(guncelle);
        }
       //[Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Guncelle(Urun model,HttpPostedFileBase File)
        {
            var urun = db.Urun.Find(model.ID);
            if (File==null)
            {
                
                urun.ad = model.ad;
                urun.Aciklama = model.Aciklama;
                urun.Fiyat = model.Fiyat;
                urun.Stok = model.Stok;
                urun.KategoriId = model.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                urun.Resim = File.FileName.ToString();
                urun.ad = model.ad;
                urun.Aciklama = model.Aciklama;
                urun.Fiyat = model.Fiyat;
                urun.Stok = model.Stok;
                urun.KategoriId = model.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            
        }


    }
}