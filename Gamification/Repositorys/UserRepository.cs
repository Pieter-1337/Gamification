using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamification.Models;
using BCrypt;

namespace Gamification.Repositorys
{
    public class UserRepository
    {
        private GamificationEntities db = new GamificationEntities();


        //Load the 2 admin accounts in the database/////////////////////////////////////////////
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
                        Password = BCrypt.Net.BCrypt.HashPassword("BrackeAdmin1993"),
                        ConfirmPassword = BCrypt.Net.BCrypt.HashPassword("BrackeAdmin1993"),
                        CountryID = 1,
                        DivisionID = 1,

                    });

                db.Users.Add(
                    new Users
                    {

                        Username = "Admin2",
                        Email = "Admin2@admin.com",
                        First_Name = "Admin2",
                        Last_Name = "Admin2",
                        Role = "Admin",
                        Password = BCrypt.Net.BCrypt.HashPassword("BrackeAdmin1993"),
                        ConfirmPassword = BCrypt.Net.BCrypt.HashPassword("BrackeAdmin1993"),
                        CountryID = 1,
                        DivisionID = 1,
                        
                    });

                db.SaveChanges();
            }
        }
      
        //////////////////////////////////////////////////////////////////////////////////
    
        //Get Users functions
        //////////////////////////////////////////////////////////////////////////////////
        public List<Users> GetUsers()
        {
            
            return db.Users.OrderBy(u => u.UserID).ToList();
        }

        

        public Users GetUser(string username, string password)
        {
            var User = (from user in db.Users where user.Username == username select user).FirstOrDefault();
            if (User != null)
            {
                bool validPassword = BCrypt.Net.BCrypt.Verify(password, User.Password);
                if (validPassword == true)
                {
                    return User;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
                
        }

        public Users GetUserById(int? id)
        {
            return db.Users.Find(id);
        }

        //Search functions for the leaderboard
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public List<Users> GetLeaderBoard()
        {
            var query = (from user in db.Users where user.Role != "Admin" orderby (user.Punten_LVL1 + user.Punten_LVL2) descending select user).ToList();
            return query;
        }

        public List<Users> GetLeaderBoardByNameAndCountryAndDivision(List<Users> LeaderBoard, string SearchString, int? SearchCountry, int? SearchDivision)
        {
            SearchString = SearchString.ToUpper();
            return (from u in LeaderBoard where (u.First_Name.ToUpper().Contains(SearchString) || u.Last_Name.ToUpper().Contains(SearchString) || u.Username.ToUpper().Contains(SearchString)) && u.Countries.CountryID.Equals(SearchCountry) && u.Divisions.DivisionID.Equals(SearchDivision) orderby (u.Punten_LVL1 + u.Punten_LVL2) descending select u).ToList();
        }

        public List<Users> GetLeaderBoardByNameAndDivision(List<Users> LeaderBoard, string SearchString, int? SearchDivision)
        {
            SearchString = SearchString.ToUpper();
            return (from u in LeaderBoard where (u.First_Name.ToUpper().Contains(SearchString) || u.Last_Name.ToUpper().Contains(SearchString) || u.Username.ToUpper().Contains(SearchString)) && u.Divisions.DivisionID.Equals(SearchDivision) orderby (u.Punten_LVL1 + u.Punten_LVL2) descending select u).ToList(); 
        }

        public List<Users> GetLeaderBoardByCountryAndDivision(List<Users> LeaderBoard, int? SearchCountry, int? SearchDivision)
        {
            return (from u in LeaderBoard where (u.Countries.CountryID.Equals(SearchCountry) && u.Divisions.DivisionID.Equals(SearchDivision)) orderby (u.Punten_LVL1 + u.Punten_LVL2) descending select u).ToList();
        }

        public List<Users> GetLeaderBoardByDivision(List<Users> LeaderBoard, int? SearchDivision)
        {
            return (from u in LeaderBoard where u.Divisions.DivisionID.Equals(SearchDivision) orderby (u.Punten_LVL1 + u.Punten_LVL2) descending select u).ToList();
        }

        public List<Users> GetLeaderBoardByNameAndCountry(List<Users> LeaderBoard, string SearchString, int? SearchCountry)
        {
            SearchString = SearchString.ToUpper();
            return (from u in LeaderBoard where (u.First_Name.ToUpper().Contains(SearchString) || u.Last_Name.ToUpper().Contains(SearchString) || u.Username.ToUpper().Contains(SearchString)) && u.Countries.CountryID.Equals(SearchCountry) orderby (u.Punten_LVL1 + u.Punten_LVL2) descending select u).ToList();
            
        }

        public List<Users> GetLeaderBoardByCountry(List<Users> LeaderBoard, int? SearchCountry)
        {
            
            return (from u in LeaderBoard where u.Countries.CountryID.Equals(SearchCountry) orderby (u.Punten_LVL1 + u.Punten_LVL2) descending select u).ToList();
        }

        public List<Users> GetLeaderBoardByName(List<Users> LeaderBoard, string SearchString)
        {
            SearchString = SearchString.ToUpper();
            return (from u in LeaderBoard where u.First_Name.ToUpper().Contains(SearchString) || u.Last_Name.ToUpper().Contains(SearchString) || u.Username.ToUpper().Contains(SearchString) orderby (u.Punten_LVL1 + u.Punten_LVL2) descending select u).ToList();
        }
     

        /////////////////////////////////////////////////////////////////////////////////////////////

       
        //Check if a Username is already taken when someone registers or creates a new user
        //////////////////////////////////////////////////////////////////////////////////////////
        public bool CheckUniqueFields(string username, string password)
        {    
            var User = (from user in db.Users where user.Username == username select user).FirstOrDefault();
            
            if(User != null)
            {
                return false;
            }
            else
            {
                return true;
            }       
         
        }
    }
}