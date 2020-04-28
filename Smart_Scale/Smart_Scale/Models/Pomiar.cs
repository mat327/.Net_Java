using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smart_Scale.Models
{
    public class Pomiar
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Pole Waga jest wymagane.")]
        public int Waga { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Pole Data dodania jest wymagane.")]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:MM}", ApplyFormatInEditMode = true)]
        public DateTime Datadodania  { get; set; }
        public double Bmi { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}