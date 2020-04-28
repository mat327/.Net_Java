using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smart_Scale.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Imię jest wymagane.")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Pole Imię może zawierać tylko litery.")]
        
        public string Imie { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Pole Imię może zawierać tylko litery.")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Pole Płeć jest wymagane.")]
        [MaxLength(1, ErrorMessage = "Można podać tylko jedną literę m- mężczyzna f- kobieta")]
        [RegularExpression("^[mf]*$", ErrorMessage = "Pole Imię może zawierać tylko litery m dla mężczyzny lub f dla kobiety.")]
        public string Plec { get; set; }

        [Required(ErrorMessage = "Pole Wzrost jest wymagane.")]
        public int Wzrost { get; set; }

        [Required(ErrorMessage = "Pole Wiek jest wymagane.")]
        public int Wiek { get; set; }
        public List<Pomiar> pomiars { get; set; }

        public User() => pomiars = new List<Pomiar>();
    }
}