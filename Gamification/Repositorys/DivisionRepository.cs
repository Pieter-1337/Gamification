﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamification.Models;

namespace Gamification.Repositorys
{
    public class DivisionRepository
    {

        private GamificationEntities db = new GamificationEntities();

        public void LoadDivisions()
        {
            var query = (from division in db.Divisions where division.Abbreviation == "HR" select division).ToList();
            if (query.Count < 1)
            {
                db.Divisions.Add(new Divisions
                {
                    Name = "Human Resources",
                    Abbreviation = "HR"
                });
                db.SaveChanges();
            }
        }

        public List<Divisions> SearchByDivision(List<Divisions> DivisionList, int? SearchDivision)
        {
            return (from d in DivisionList where d.DivisionID.Equals(SearchDivision) orderby d.Name select d).ToList();
        }
    }
}