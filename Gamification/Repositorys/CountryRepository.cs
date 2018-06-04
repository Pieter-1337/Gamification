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
                    Name = "Belgium",
                    Abbreviation = "BE",
                });
                db.SaveChanges();
            }
        }

        public List<Countries> SearchByCountry(List<Countries> CountryList , int? SearchCountry)
        {
            return (from c in CountryList where c.CountryID.Equals(SearchCountry) orderby c.Name select c).ToList();
        }
    }
}