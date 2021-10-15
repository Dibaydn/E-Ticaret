using E_Ticaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret.Controllers
{
   //[Authorize(Roles = "A")]
    public class KategoriController : Controller
    {
        // GET: Kategori

        Entities1 db = new Entities1();
       
        public ActionResult Index()
        {
            return View(db.Kategori.Where(x=>x.durum==true).ToList());
        }
        //[Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Ekle(Kategori data)
        {
            db.Kategori.Add(data);
            data.durum = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //[Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            var kategori = db.Kategori.Where(x => x.Id == id).FirstOrDefault();
            db.Kategori.Remove(kategori);
            kategori.durum = false;
            
            db.SaveChanges();
            return RedirectToAction("Index");

        }
       // [Authorize(Roles = "A")]
        public ActionResult Guncelle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Guncelle(Kategori data)
        {
            var guncelle = db.Kategori.Where(x => x.Id == data.Id).FirstOrDefault();
            guncelle.Aciklama = data.Aciklama;
            guncelle.Ad= data.Ad;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}