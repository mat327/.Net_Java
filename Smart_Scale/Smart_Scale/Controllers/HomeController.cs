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
using log4net;

namespace Smart_Scale.Controllers
{
    public class HomeController : Controller
    {
        private SmartWeightDbContext db = new SmartWeightDbContext();
        private static Client client = Client.Instance;
        private static readonly ILog Log = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            try
            {
                Log.Info("Wyświetlenie strony głównej");
                return View();
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult Users()
        {
            try
            {
                Log.Info("Wyświetlenie listy użytkowników");
                ViewBag.Message = "Użytkownicy serwisu";

                return View(db.users.ToList());
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult CreateUser()
        {
            try
            {
                Log.Info("Wyświetlenie formularza tworzenia użytkownika");

                return View();
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "Id,Imie,Nazwisko,Plec,Wzrost,Wiek")] User user)
        {
            try
            {
                Log.Debug("Przekazany obiekt : "+user.Imie + " " + user.Nazwisko + " " + user.Plec + " " + user.Wzrost + " " + user.Wiek);
                if (ModelState.IsValid)
                {
                    Log.Info("Przekazany obiekt jest prawidłowy");
                    db.users.Add(user);
                    db.SaveChanges();
                    Log.Info("Użytkownik dodany do bazy danych");
                    return RedirectToAction("Users");
                }
                Log.Warn("Przekazany obiekt jest nieprawidłowy");
                Log.Info("Wyświetlenie formularza tworzenia użytkownika");
                return View(user);
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult Pomiary(int? userid)
        {
            try
            {
                Log.Info("Wywołanie strony z pomiarami użytkownika o id = "+userid);
                int? id = userid;
                User user = db.users.Find(id);
                ViewBag.Message = "Pomiary użytkownika " + user.Imie + " " + user.Nazwisko;
                Log.Info("Znaleziono użytkownika o id = " + user.Id);
                var pomiary = from m in db.pomiars
                              select m;
                pomiary = pomiary.Where(s => s.UserId == userid);
                Log.Info("Znaleziono pomiary użytkownika o id = " + user.Id);
                Log.Info("Wyświetlenie listy pomiarów użytkownika o id = " + user.Id);
                return View(pomiary.ToList());
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult Dodajpomiar(int? userid)
        {
            try
            {
                Log.Info("Wyświetlenie formularza dodawania pomiaru dla użytkownika o  id = "+userid);
                int? id = userid;
                User user = db.users.Find(id);
                ViewBag.Message = "Dodaj pomiar dla użytkownika " + user.Imie + " " + user.Nazwisko;
                Log.Info("Znaleziono użytkownika o id = " + user.Id);
                var model = new Pomiar() { Datadodania = DateTime.Now };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodajpomiar([Bind(Include = "Id,Waga,Datadodania,UserId")] Pomiar pomiar)
        { 
            try
            {
                Log.Debug("Przekazany model : " + pomiar.Waga + " " + pomiar.Datadodania + " " + pomiar.UserId);
                User user = db.users.Find(pomiar.UserId);
                Log.Info("Znaleziono użytkownika o id = "+user.Id);
                string Waga = pomiar.Waga.ToString(), Wiek = user.Wiek.ToString(), Plec = user.Plec, Wzrost = user.Wzrost.ToString();
                string response = client.Post2(Waga, Wiek, Plec, Wzrost);
                Log.Debug("Otrzymana odp od zew API : "+ response);

                DTO dto = JsonConvert.DeserializeObject<DTO>(response);
                Log.Info("Odp zew API zapisana do obiektu dto");
                pomiar.Bmi = double.Parse(dto.bmi.value, System.Globalization.CultureInfo.InvariantCulture);
                Log.Info("Dodano BMI do obiektu pomiar, BMI = " + pomiar.Bmi);

                ViewBag.Message = "Dodaj pomiar dla użytkownika " + user.Imie + " " + user.Nazwisko;
                if (ModelState.IsValid)
                {
                    Log.Info("Przekazany obiekt jest prawidłowy");
                    db.pomiars.Add(pomiar);
                    db.SaveChanges();
                    Log.Info("Pomiar dodany do bazy danych");
                    ViewBag.MessageAdd = "Pomiar został dodany.";
                    ViewBag.Bmi = "Twoje Bmi jest równe " + pomiar.Bmi + " .";
                    ViewBag.IdealWeight = "Idealna waga dla użytkownika " + user.Imie + " " + user.Nazwisko + " : " + dto.ideal_weight;
                    ViewBag.Risk = dto.bmi.risk;
                    ViewBag.Status = dto.bmi.status;
                    Log.Info("Wyświetlenie informacji o dodanym pomiarze dla użytkownika o  id = " + pomiar.UserId);
                }
                else
                {
                    Log.Warn("Przekazany obiekt jest nieprawidłowy");
                    Log.Info("Wyświetlenie formularza dodawania pomiaru dla użytkownika o  id = " + pomiar.UserId);
                }

                return View(pomiar);
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }

        }

        public ActionResult Delete(int id, int? userid)
        {
            try
            {
                Log.Info("Wywoałno usunięcie pomiaru id ="+ id + ", użytkownika o id = "+userid);
                Pomiar pomiar = db.pomiars.Find(id);
                Log.Info("Znaleziono pomiar o id = " + pomiar.Id);
                db.pomiars.Remove(pomiar);
                db.SaveChanges();
                Log.Info("Pomiar o id = " + pomiar.Id + " został usuniety z bazy danych");
                return RedirectToAction("Pomiary", new { userid = userid });
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult Wykres(string Imie, int? userid, string Nazwisko)
        {
            try
            {
                Log.Info("Wywołanie utworzenia wykresu wagi dla użytkownika o id = "+userid);
                ViewBag.Message = "Wykres wagi użytkownika " + Imie + " " + Nazwisko;
                var pomiary = from m in db.pomiars
                              select m;
                pomiary = pomiary.Where(s => s.UserId == userid);
                Log.Info("Pobrano pomiary dla użytkownika o id = " + userid + " z bazy danych");
                return View(pomiary.ToList());
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult Wykres2(string Imie, int? userid, string Nazwisko)
        {
            try
            {
                Log.Info("Wywołanie utworzenia wykresu wskażnika BMI dla użytkownika o id = " + userid);
                ViewBag.Message = "Wykres wskażnika BMI użytkownika " + Imie + " " + Nazwisko;
                var pomiary = from m in db.pomiars
                              select m;
                pomiary = pomiary.Where(s => s.UserId == userid);
                Log.Info("Pobrano pomiary dla użytkownika o id = " + userid + " z bazy danych");
                return View(pomiary.ToList());
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult ShowWykres(string Imie, int? userid, string Nazwisko)
        {
            try
            {
                Log.Info("Wyświetlenie wykresów dla użytkownika o id = " + userid);
                ViewBag.Message = "Wykres pomiarów użytkownika " + Imie + " " + Nazwisko;
                ViewBag.Wykres = "Wykres?imie=" + Imie + "&userid=" + userid + "&nazwisko=" + Nazwisko;
                ViewBag.Wykres2 = "Wykres2?imie=" + Imie + "&userid=" + userid + "&nazwisko=" + Nazwisko;
                return View();
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult About()
        {
            try
            {
                Log.Info("Wyświetlenie strony Informacje");
                ViewBag.Message = "Informacje o serwisie.";

                return View();
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }

        public ActionResult Contact()
        {
            try
            {
                Log.Info("Wyświetlenie strony Kontakt");
                ViewBag.Message = "Kontakt do administracji serwisu.";

                return View();
            }
            catch (Exception ex)
            {
                Log.Error("Hi I am log4net Error Level", ex);
                Log.Fatal("Hi I am log4net Fatal Level", ex);
                throw;
            }
        }
    }
}
