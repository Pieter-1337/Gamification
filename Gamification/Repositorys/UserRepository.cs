using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamification.Models;

namespace Gamification.Repositorys
{
    public class UserRepository
    {
        private GamificationEntities db = new GamificationEntities();

        //Load the 2 admin accounts in the database
        public void LoadAdmins()
        {
            var query = (from user in db.Users where user.Username == "Admin1" select user).ToList();
            if (query.Count < 1)
            {
                db.Users.Add(
                    new Users
                    {
                        
                        Username = "Admin1",
                        Email = "Admin1@admin.com",
                        First_Name = "Admin1",
                        Last_Name = "Admin1",
                        Role = "Admin",
                        Country = "BE",
                        Division = "Admin",
                        Password = "BrackeAdmin1993",

                    });

                db.Users.Add(
                    new Users
                    {

                        Username = "Admin2",
                        Email = "Admin2@admin.com",
                        First_Name = "Admin2",
                        Last_Name = "Admin2",
                        Role = "Admin",
                        Country = "BE",
                        Division = "Admin",
                        Password = "BrackeAdmin1993",

                    });

                db.SaveChanges();
            }
        }

        public List<Users> GetUsers()
        {
            return db.Users.OrderBy(u => u.UserID).ToList();
        }

        public Users GetUser(string username, string password)
        {
            var User = (from user in db.Users where user.Username == username && user.Password == password select user).FirstOrDefault();
            return User;
        }
    }
}