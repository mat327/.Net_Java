using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Smart_Scale.Models
{
    public class SmartWeightDbContext : DbContext
    {
        public DbSet<User> users{ get; set; }
        public DbSet<Pomiar> pomiars { get; set; }
    }
}