using E_Ticaret.Models;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using static System.Exception;
using System;

namespace E_Ticaret.Controllers
{
    public class SatisController : Controller
    {
        // GET: Satis

        Entities1 db = new Entities1();
        public ActionResult Index(int sayfa=1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.Email == kullaniciadi);
                var model = db.Satıslar.Where(x => x.KullaniciId == kullanici.Id).ToList().ToPagedList(sayfa, 5);
                return View(model);


            }
            return HttpNotFound();
        }
        public ActionResult SatinAl(int id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.ID== id);
            return View(model);

        }
        [HttpPost]
        public ActionResult SatinAl2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepet.FirstOrDefault(x => x.ID == id);
                    var satis = new Satıslar
                    {
                        KullaniciId = model.KullanıcıId,
                        UrunId = model.UrunId,
                        Adet = model.Adet,
                        Resim = model.Resim,
                        fiyat = model.Fiyat,
                        Tarih = model.Tarih
                    };
                    db.Sepet.Remove(model);
                    db.Satıslar.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = ("satın Alma İşleminiz Başarıyla Gerçekleşmiştir.");

                }
            }
            catch (Exception)
            {
                ViewBag.islem = "Satın Alma İşleminiz Başarısız";
            }
            return View("islem");
        }
        public ActionResult HepsiniSatinAl(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.Email == kullaniciadi);
                var model = db.Sepet.Where(x => x.KullanıcıId == kullanici.Id).ToList();
                var kid = db.Sepet.FirstOrDefault(x => x.KullanıcıId == kullanici.Id);
                if (model!=null)
                {
                    if (kid==null)
                    {
                        ViewBag.Tutar = "Sepetinizde Ürün Bulunmamaktadır...";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.Sepet.Where(x => x.KullanıcıId == kid.KullanıcıId).Sum(x => x.Urun.Fiyat * x.Adet);
                        ViewBag.Tutar = "Toplam Tutar=" + Tutar + "TL";
                    }
                    return View(model);

                }
                return View();

            }
            return HttpNotFound();

        }
        [HttpPost]

        public ActionResult HepsiniSatinAl2()
        {
            var username = User.Identity.Name;
            var kullanici = db.Kullanici.FirstOrDefault(x => x.Email == username);
            var model = db.Sepet.Where(x => x.KullanıcıId == kullanici.Id).ToList();
            int satir = 0;
            foreach (var item in model)
            {
                var satis = new Satıslar
                {
                    KullaniciId = model[satir].KullanıcıId,
                    UrunId = model[satir].UrunId,
                    Adet = model[satir].Adet,
                    fiyat = model[satir].Fiyat,
                    Resim = model[satir].Urun.Resim,
                    Tarih = DateTime.Now,

                };
                db.Satıslar.Add(satis);
                db.SaveChanges();
                satir++;
            }
            db.Sepet.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }


    }
}