using Smart_Scale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Web.Helpers;

namespace Smart_Scale.Controllers
{
    public class HomeController : Controller
    {
        private SmartWeightDbContext db = new SmartWeightDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            ViewBag.Message = "Użytkownicy serwisu";

            return View(db.users.ToList());
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "Id,Imie,Nazwisko")] User user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Users");
            }

            return View(user);
        }

        public ActionResult Pomiary(int? userid)
        {
            int? id = userid;
           User user = db.users.Find(id);
            ViewBag.Message = "Pomiary użytkownika " + user.Imie + " " + user.Nazwisko;
            var pomiary = from m in db.pomiars
                            select m;
            pomiary = pomiary.Where(s => s.UserId == userid);
            return View(pomiary.ToList());
        }

        public ActionResult Dodajpomiar(int? userid)
        {
            int? id = userid;
            User user = db.users.Find(id);
            ViewBag.Message = "Dodaj pomiar dla użytkownika " + user.Imie + " " + user.Nazwisko;
            var model = new Pomiar() { Datadodania= DateTime.Now};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodajpomiar([Bind(Include = "Waga,Datadodania,UserId")] Pomiar pomiar)
        {
            ViewBag.Message =pomiar.Id + "   " + pomiar.Waga + "   " + pomiar.Datadodania + "   " + pomiar.UserId;
         if (ModelState.IsValid)
            {
                db.pomiars.Add(pomiar);
                db.SaveChanges();
                ViewBag.MessageAdd = "Pomiar został dodany.";
            }

            return View(pomiar);
        }

        public ActionResult Delete(int id, int? userid)
        {
            Pomiar pomiar = db.pomiars.Find(id);
            db.pomiars.Remove(pomiar);
            db.SaveChanges();
            return RedirectToAction("Pomiary", new { userid = userid });
        }

        public ActionResult Wykres(string Imie, int? userid, string Nazwisko)
        {
            ViewBag.Message = "Wykres pomiarów użytkownika "+ Imie + " " + Nazwisko;
            var pomiary = from m in db.pomiars
                          select m;
            pomiary = pomiary.Where(s => s.UserId == userid);
           return View(pomiary.ToList());
        }

        public ActionResult ShowWykres(string Imie, int? userid, string Nazwisko)
        {
            ViewBag.Message = "Wykres pomiarów użytkownika " + Imie + " " + Nazwisko;
            ViewBag.Wykres = "Wykres?imie="+Imie+"&userid="+userid+"&nazwisko="+Nazwisko;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Informacje o serwisie.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt do administracji serwisu.";

            return View();
        }
    }
}