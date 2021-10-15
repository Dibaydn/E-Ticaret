using E_Ticaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace E_Ticaret.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        Entities1 db = new Entities1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Login(Kullanici p)
        {
            var bilgiler = db.Kullanici.FirstOrDefault(x => x.Email == p.Email && x.Sifre == p.Sifre);
            if (bilgiler!=null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.Email, false);
                Session["Mail"] = bilgiler.Email.ToString();
                Session["Ad"] = bilgiler.Ad.ToString();
                Session["Soyad"] = bilgiler.Soyad.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.hata = "Kullanıcı Adı veya Şifre Hatalı!";
            }
            return View();
        }
       
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Kullanici data)
        {
            db.Kullanici.Add(data);
            data.Rol = "U";
            db.SaveChanges();
            return RedirectToAction("Login", "Account");
        }

    }
}