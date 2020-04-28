using Smart_Scale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Web.Helpers;
using Smart_Scale.Clients;
using Newtonsoft.Json;

namespace Smart_Scale.Controllers
{
    public class HomeController : Controller
    {
        private SmartWeightDbContext db = new SmartWeightDbContext();
        private static Client client = Client.Instance;

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
        public ActionResult CreateUser([Bind(Include = "Id,Imie,Nazwisko,Plec,Wzrost,Wiek")] User user)
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
        public ActionResult Dodajpomiar([Bind(Include = "Id,Waga,Datadodania,UserId")] Pomiar pomiar)
        {
            User user = db.users.Find(pomiar.UserId);
            string Waga = pomiar.Waga.ToString(), Wiek = user.Wiek.ToString(), Plec = user.Plec, Wzrost = user.Wzrost.ToString(); 
            string response = client.Post2(Waga, Wiek, Plec, Wzrost);

            DTO dto = JsonConvert.DeserializeObject<DTO>(response);
            pomiar.Bmi = double.Parse(dto.bmi.value, System.Globalization.CultureInfo.InvariantCulture);

            ViewBag.Message = "Dodaj pomiar dla użytkownika " + user.Imie + " " + user.Nazwisko;
            if (ModelState.IsValid)
           {
                db.pomiars.Add(pomiar);
                db.SaveChanges();
                ViewBag.MessageAdd = "Pomiar został dodany.";
                ViewBag.Bmi = "Twoje Bmi jest równe " + pomiar.Bmi+" .";
                ViewBag.IdealWeight = "Idealna waga dla użytkownika " + user.Imie + " " + user.Nazwisko + " : " + dto.ideal_weight;
                ViewBag.Risk = dto.bmi.risk;
                ViewBag.Status = dto.bmi.status;
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
            ViewBag.Message = "Wykres wagi użytkownika "+ Imie + " " + Nazwisko;
            var pomiary = from m in db.pomiars
                          select m;
            pomiary = pomiary.Where(s => s.UserId == userid);
           return View(pomiary.ToList());
        }

        public ActionResult Wykres2(string Imie, int? userid, string Nazwisko)
        {
            ViewBag.Message = "Wykres wskaźnika BMI użytkownika " + Imie + " " + Nazwisko;
            var pomiary = from m in db.pomiars
                          select m;
            pomiary = pomiary.Where(s => s.UserId == userid);
            return View(pomiary.ToList());
        }

        public ActionResult ShowWykres(string Imie, int? userid, string Nazwisko)
        {
            ViewBag.Message = "Wykres pomiarów użytkownika " + Imie + " " + Nazwisko;
            ViewBag.Wykres = "Wykres?imie="+Imie+"&userid="+userid+"&nazwisko="+Nazwisko;
            ViewBag.Wykres2 = "Wykres2?imie=" + Imie + "&userid=" + userid + "&nazwisko=" + Nazwisko;
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