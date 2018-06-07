using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamification.Models;

namespace Gamification.Repositorys
{
    public class BadgeRepository
    {
        private GamificationEntities db = new GamificationEntities();

        public List<Badge> SearchByBadge(List<Badge> BadgeList, int? SearchBadge)
        {
            return (from b in BadgeList where b.BadgeID.Equals(SearchBadge) orderby b.Name select b).ToList();
        }
    }

   
}