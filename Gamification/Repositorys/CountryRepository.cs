using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamification.Models;

namespace Gamification.Repositorys
{
    public class CountryRepository
    {
        private GamificationEntities db = new GamificationEntities();

        public void LoadCountries()
        {
            var query = (from country in db.Countries where country.Abbreviation == "BE" select country).ToList();
            if(query.Count < 1)
            {
                db.Countries.Add(new Countries
                {
                    Name = "Belguim",
                    Abbreviation = "BE",
                });
                db.SaveChanges();
            }
        }
    }
}