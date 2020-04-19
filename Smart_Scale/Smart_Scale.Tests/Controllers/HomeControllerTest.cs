using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart_Scale;
using Smart_Scale.Controllers;
using Smart_Scale.Models;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Helpers;

namespace Smart_Scale.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private SmartWeightDbContext db = new SmartWeightDbContext();

        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Informacje o serwisie.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Pomiary()
        {
            // Arrange
            HomeController controller = new HomeController();
            int? zmienna = 1;
            // Act
            ViewResult result = controller.Pomiary(zmienna) as ViewResult;

            // Assert
            Assert.AreEqual("Pomiary użytkownika Mateusz Białek", result.ViewBag.Message);
        }

        [TestMethod]
        public void CreateUser()
        {
            // Arrange
            HomeController controller = new HomeController();
            string Imie = "ABCD";
            string Nazwisko = "ABCD";
            User user = new User() {Imie = Imie, Nazwisko = Nazwisko };

            // Act
            db.users.Add(user);
            db.SaveChanges();
            var users = from m in db.users
                          select m;
            User userfromdb = users.First(s => s.Imie == Imie && s.Nazwisko ==Nazwisko);
            Console.WriteLine("ID "+userfromdb.Id);
            db.users.Remove(userfromdb); //cleaning from db
            db.SaveChanges();

            // Assert
            Assert.AreEqual("ABCD", userfromdb.Imie);
        }

        [TestMethod]
        public void Delete()
        {
            HomeController controller = new HomeController();
            // Arrange
            string Imie = "ABCD"; //Add user
            string Nazwisko = "ABCD";
            User user = new User() { Imie = Imie, Nazwisko = Nazwisko };
            db.users.Add(user);
            db.SaveChanges();
            var users = from m in db.users
                        select m;
            User userfromdb = users.First(s => s.Imie == Imie && s.Nazwisko == Nazwisko);
            int IDużytkownika = userfromdb.Id;


            decimal Waga = 10200; //Add pomiar
            DateTime Datadodania = new DateTime(2008, 5, 1, 8, 30, 52);
            Pomiar pomiar = new Pomiar() { Waga = Waga, Datadodania = Datadodania, UserId = IDużytkownika };
            db.pomiars.Add(pomiar);
            db.SaveChanges();
            var pomiars = from m in db.pomiars
                          select m;
            Pomiar pomiarfromdb = pomiars.First(s => s.Waga == Waga && s.Datadodania == Datadodania);
            int Iloscprzed = db.pomiars.Count();

            // Act
            db.pomiars.Remove(pomiarfromdb);
            db.users.Remove(userfromdb);//cleaning from db
            db.SaveChanges();
            int Iloscpo = db.pomiars.Count();

            // Assert
            Assert.AreEqual(Iloscpo+1, Iloscprzed);
        }
    }
}
