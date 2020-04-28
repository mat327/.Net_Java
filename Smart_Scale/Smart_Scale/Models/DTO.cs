using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Smart_Scale.Models
{
    public class DTO
    {
        public Weight weight { get; set; }
        public Height height { get; set; }
        public BMI bmi { get; set; }
        public string ideal_weight { get; set; }
        public string surface_area { get; set; }
        public string ponderal_index { get; set; }
        public BMR bmr { get; set; }
        public WHR whr { get; set; }
        public WHTR whtr { get; set; }
    }

    public class Weight
    {
        public string kg { get; set; }
        public string lb { get; set; }
    }

    public class Height
    {
        public string m { get; set; }
        public string cm { get; set; }

        [JsonProperty(PropertyName = "in")]
        public string inc { get; set; }
        [JsonProperty(PropertyName = "ft-in")]
        public string ft_inc { get; set; }

    }

    public class BMI
    {
        public string value { get; set; }
        public string status { get; set; }
        public string risk { get; set; }
        public string prime { get; set; }
    }

    public class BMR
    {
        public string value { get; set; }
    }

    public class WHR
    {
        public string value { get; set; }
        public string status { get; set; }
    }

    public class WHTR
    {
        public string value { get; set; }
        public string status { get; set; }
    }
}
