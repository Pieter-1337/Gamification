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
        public bool CheckUniqueFieldsUsername(string username, string password)
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

        public bool CheckUniqueFieldsEmail(string email)
        {
            var User = (from user in db.Users where user.Email.ToLower() == email.ToLower() select user).FirstOrDefault();

            if (User != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Check if old pass matches the one in the db if true replace it with new pass
        //////////////////////////////////////////////////////////////////////////
        public bool UpdatePassword(int id, string Oldpassword, string Newpassword)
        {
            var user = GetUserById(id);
            if(BCrypt.Net.BCrypt.Verify(Oldpassword, user.Password))
            {
                var EncryptedPass = BCrypt.Net.BCrypt.HashPassword(Newpassword);
                user.Password = EncryptedPass;
                user.ConfirmPassword = EncryptedPass;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //ADMIN STATISTICS PANEL
        ////////////////////////////////////////////////////////////////////////////////////

        //People registered in the whole app
        public int RegisteredPeopleWholeApp(List<Users> users)
        {
            return (from u in users select u).Count();
        }

        //People registered per country
        public Dictionary<string, int> RegisteredPeoplePerCountry(List<Users> users)
        {
            Dictionary<string, int> Mydict = new Dictionary<string, int>(); 

            foreach(var country in db.Countries)
            {
                
                int CountUsersPerCountry = (from u in users where u.CountryID == country.CountryID select u).Count();
                Mydict.Add(country.Name, CountUsersPerCountry);
                
            }
            return Mydict;         
        }

        //People registered per country By division
        public Dictionary<string, int> RegisteredPeoplePerCountryByDivision(List<Users> users, int? SearchDivision)
        {
            Dictionary<string, int> Mydict = new Dictionary<string, int>();

            foreach (var country in db.Countries)
            {

                int CountUsersPerCountry = (from u in users where u.CountryID == country.CountryID && u.DivisionID == SearchDivision select u).Count();
                Mydict.Add(country.Name, CountUsersPerCountry);

            }
            return Mydict;
        }

        //People registered per division
        public Dictionary<string , int> RegisteredPeoplePerDivision(List<Users> users)
        {
            Dictionary<string, int> Mydict = new Dictionary<string, int>();

            foreach(var division in db.Divisions)
            {
               
                int CountUsersPerDivision = (from u in users where u.DivisionID == division.DivisionID select u).Count();
                Mydict.Add(division.Name, CountUsersPerDivision);
            }
            return Mydict;
        }

        //People registered per division By Country
        public Dictionary<string, int> RegisteredPeoplePerDivisionByCountry(List<Users> users, int? SearchCountry)
        {
            Dictionary<string, int> Mydict = new Dictionary<string, int>();

            foreach (var division in db.Divisions)
            {

                int CountUsersPerDivision = (from u in users where u.DivisionID == division.DivisionID && u.CountryID == SearchCountry select u).Count();
                Mydict.Add(division.Name, CountUsersPerDivision);
            }
            return Mydict;
        }

        //BADGES EARNED
        ////////////////////////////////////////

        //Badges earned in the whole app
        public int BadgesEarnedWholeApp(List<Users> users)
        {
           int BadgesEarned = 0;
           var List = (from u in users where (u.BadgeID != null) && (u.Badges.BadgeLevel > 0) select u);
            foreach(var user in List)
            {
                if(user.Badges.BadgeLevel == 1)
                {
                    BadgesEarned++;
                }
                if(user.Badges.BadgeLevel == 2)
                {
                    BadgesEarned = BadgesEarned + 2;
                }
            }

            return BadgesEarned;
        }

        //Badges earned Level1
        public int BadgesEarnedLevel1(List<Users> users)
        {
            return (from u in users where (u.BadgeID != null) && (u.Badges.BadgeLevel > 0) select u).Count();
        }

        //Badges earned Level2
        public int BadgesEarnedLevel2(List<Users> users)
        {
            return (from u in users where (u.BadgeID != null) && (u.Badges.BadgeLevel > 1) select u).Count();
        }

        //Badges Earned Level 1 per country
        public Dictionary<string, int> BadgesEarnedLevel1PerCountry(List<Users> users)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();

            foreach(var country in db.Countries)
            {
                int CountUsersWithBadgeLevel1OrHigherPerCountry = (from u in users where (u.CountryID == country.CountryID) && (u.BadgeID != null) && (u.Badges.BadgeLevel > 0) select u).Count();
                MyDict.Add(country.Name, CountUsersWithBadgeLevel1OrHigherPerCountry);
            }
            return MyDict;
        }

        //Badges Earned Level 1 per country By Division
        public Dictionary<string, int> BadgesEarnedLevel1PerCountryPerDivision(List<Users> users, int? SearchDivision)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();

            foreach(var country in db.Countries)
            {
                int CountUsersWithBadgeLevel1OrHigherPerCountryPerDivision = (from u in users where (u.CountryID == country.CountryID) && (u.BadgeID != null) && (u.Badges.BadgeLevel > 0) && (u.DivisionID == SearchDivision) select u).Count();
                MyDict.Add(country.Name, CountUsersWithBadgeLevel1OrHigherPerCountryPerDivision);
            }
            return MyDict;
        }

        //Badges Earned Level 1 per Division
        public Dictionary<string, int> BadgesEarnedLevel1PerDivision(List<Users> users)
        {
            Dictionary<string, int> Mydict = new Dictionary<string, int>();

            foreach (var division in db.Divisions)
            {

                int CountUsersWithBadgeLevel1OrHigherPerDivision = (from u in users where (u.DivisionID == division.DivisionID) && (u.BadgeID != null) && (u.Badges.BadgeLevel > 0) select u).Count();
                Mydict.Add(division.Name, CountUsersWithBadgeLevel1OrHigherPerDivision);
            }
            return Mydict;
        }

        //Badges Earned Level 1 per Division By Country
        public Dictionary<string, int> BadgesEarnedLevel1PerDivisionPerCountry(List<Users> users, int? SearchCountry)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();

            foreach(var division in db.Divisions)
            {
                int CountUsersWithBadgeLevel1OrHigherPerDivisionPerCountry = (from u in users where (u.DivisionID == division.DivisionID) && (u.BadgeID != null) && (u.Badges.BadgeLevel > 0) && (u.CountryID == SearchCountry) select u).Count();
                MyDict.Add(division.Name, CountUsersWithBadgeLevel1OrHigherPerDivisionPerCountry);
            }

            return MyDict;
        }

        //Badges Earned Level 2 per Country
        public Dictionary<string, int> BadgesEarnedLevel2PerCountry(List<Users> users)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();

            foreach (var country in db.Countries)
            {
                int CountUsersWithBadgeLevel2OrHigherPerCountry = (from u in users where (u.CountryID == country.CountryID) && (u.BadgeID != null) && (u.Badges.BadgeLevel > 1) select u).Count();
                MyDict.Add(country.Name, CountUsersWithBadgeLevel2OrHigherPerCountry);
            }
            return MyDict;
        }

        //Badges Earned Level 2 Per Country By Division
        public Dictionary<string, int> BadgesEarnedLevel2PerCountryPerDivision(List<Users> users,int? SearchDivision)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();

            foreach(var country in db.Countries)
            {
                int CountUsersWithBadgeLevel2OrHigherPerCountryPerDivision = (from u in users where (u.CountryID == country.CountryID) && (u.BadgeID != null) && (u.Badges.BadgeLevel > 1) && (u.DivisionID == SearchDivision) select u).Count();
                MyDict.Add(country.Name, CountUsersWithBadgeLevel2OrHigherPerCountryPerDivision);
            }

            return MyDict;
        }

        //Badges Earned Level 2 per Division
        public Dictionary<string, int> BadgesEarnedLevel2PerDivision(List<Users> users)
        {
            Dictionary<string, int> Mydict = new Dictionary<string, int>();

            foreach (var division in db.Divisions)
            {

                int CountUsersWithBadgeLevel2OrHigherPerDivision = (from u in users where (u.DivisionID == division.DivisionID) && (u.BadgeID != null) && (u.Badges.BadgeLevel > 1) select u).Count();
                Mydict.Add(division.Name, CountUsersWithBadgeLevel2OrHigherPerDivision);
            }
            return Mydict;
        }

        //Badges Earned Level 2 Per division By Country
        public Dictionary<string, int> BadgesEarnedLevel2PerDivisionPerCountry(List<Users> users,int? SearchCountry)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach(var division in db.Divisions)
            {
                int CountUsersWithBadgeLevel2OrHigherPerDivisionPerCountry = (from u in users where (u.DivisionID == division.DivisionID) && (u.BadgeID != null) && (u.Badges.BadgeLevel > 1) && (u.CountryID == SearchCountry) select u).Count();
                MyDict.Add(division.Name, CountUsersWithBadgeLevel2OrHigherPerDivisionPerCountry);
            }

            return MyDict;
        }

        //Points earned in the whole app
        public int PointsEarnedWholeApp(List<Users> users)
        {
            int points = 0;
            foreach(var user in users)
            {
                int? TempPoints = user.Punten_LVL1 + user.Punten_LVL2;
                if(TempPoints != null)
                {
                    points += Convert.ToInt32(TempPoints);
                }              
            }
            return points;
        }

        //Points earned Level 1 Total
        public int PointsEarnedLevel1(List<Users> users)
        {
            int points = 0;
            foreach (var user in users)
            {
                int? TempPoints = user.Punten_LVL1;
                if (TempPoints != null)
                {
                    points += Convert.ToInt32(TempPoints);
                }
            }
            return points;
        }

        //Points earned Level 1 per country
        public Dictionary<string, int> PointsEarnedLevel1PerCountry(List<Users> users)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach(var country in db.Countries)
            {
                var List = (from u in users where country.CountryID == u.CountryID select u).ToList();
                int points = 0;          
                    foreach (var user in List)
                    {
                        int? TempPoints = user.Punten_LVL1;
                        if (TempPoints != null)
                        {
                            points += Convert.ToInt32(TempPoints);
                        }
                    }
                MyDict.Add(country.Name, points);
            }

            return MyDict;
        }

        //Points earned Level1 per Country Per division
        public Dictionary<string, int> PointsEarnedLevel1PerCountryPerDivision(List<Users> users,int? SearchDivision)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach (var country in db.Countries)
            {
                var List = (from u in users where (country.CountryID == u.CountryID) && (u.DivisionID == SearchDivision) select u).ToList();
                int points = 0;
                foreach (var user in List)
                {
                    int? TempPoints = user.Punten_LVL1;
                    if (TempPoints != null)
                    {
                        points += Convert.ToInt32(TempPoints);
                    }
                }
                MyDict.Add(country.Name, points);
            }

            return MyDict;
        }

        //Points earned Level 1 per Division
        public Dictionary<string, int> PointsEarnedLevel1PerDivision(List<Users> users)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach (var division in db.Divisions)
            {
                var List = (from u in users where division.DivisionID == u.DivisionID select u).ToList();
                int points = 0;
                foreach (var user in List)
                {
                    int? TempPoints = user.Punten_LVL1;
                    if (TempPoints != null)
                    {
                        points += Convert.ToInt32(TempPoints);
                    }
                }
                MyDict.Add(division.Name, points);
            }

            return MyDict;
        }

        //Points earned Level 1 Per Division Per Country
        public Dictionary<string, int> PointsEarnedLevel1PerDivisionPerCountry(List<Users> users,int? SearchCountry)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach (var division in db.Divisions)
            {
                var List = (from u in users where (division.DivisionID == u.DivisionID) && (u.CountryID == SearchCountry) select u).ToList();
                int points = 0;
                foreach (var user in List)
                {
                    int? TempPoints = user.Punten_LVL1;
                    if (TempPoints != null)
                    {
                        points += Convert.ToInt32(TempPoints);
                    }
                }
                MyDict.Add(division.Name, points);
            }

            return MyDict;
        }

        //Points earned Level 2 Total
        public int PointsEarnedLevel2(List<Users> users)
        {
            int points = 0;
            foreach (var user in users)
            {
                int? TempPoints = user.Punten_LVL2;
                if (TempPoints != null)
                {
                    points += Convert.ToInt32(TempPoints);
                }
            }
            return points;
        }

        //Points earned Level 2 per Country
        public Dictionary<string, int> PointsEarnedLevel2PerCountry(List<Users> users)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach (var country in db.Countries)
            {
                var List = (from u in users where country.CountryID == u.CountryID select u).ToList();
                int points = 0;
                foreach (var user in List)
                {
                    int? TempPoints = user.Punten_LVL2;
                    if (TempPoints != null)
                    {
                        points += Convert.ToInt32(TempPoints);
                    }
                }
                MyDict.Add(country.Name, points);
            }

            return MyDict;
        }

        //Points earned Level 2 per Country Per division
        public Dictionary<string, int> PointsEarnedLevel2PerCountryPerDivision(List<Users> users, int? SearchDivision)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach (var country in db.Countries)
            {
                var List = (from u in users where (country.CountryID == u.CountryID) && (u.DivisionID == SearchDivision) select u).ToList();
                int points = 0;
                foreach (var user in List)
                {
                    int? TempPoints = user.Punten_LVL2;
                    if (TempPoints != null)
                    {
                        points += Convert.ToInt32(TempPoints);
                    }
                }
                MyDict.Add(country.Name, points);
            }

            return MyDict;
        }

        //Points earned Level 2 per division
        public Dictionary<string, int> PointsEarnedLevel2PerDivision(List<Users> users)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach (var division in db.Divisions)
            {
                var List = (from u in users where division.DivisionID == u.DivisionID select u).ToList();
                int points = 0;
                foreach (var user in List)
                {
                    int? TempPoints = user.Punten_LVL2;
                    if (TempPoints != null)
                    {
                        points += Convert.ToInt32(TempPoints);
                    }
                }
                MyDict.Add(division.Name, points);
            }

            return MyDict;
        }

        //Points earend level 2 per division per Country
        public Dictionary<string, int> PointsEarnedLevel2PerDivisionPerCountry(List<Users> users, int? SearchCountry)
        {
            Dictionary<string, int> MyDict = new Dictionary<string, int>();
            foreach (var division in db.Divisions)
            {
                var List = (from u in users where (division.DivisionID == u.DivisionID) && (u.CountryID == SearchCountry) select u).ToList();
                int points = 0;
                foreach (var user in List)
                {
                    int? TempPoints = user.Punten_LVL2;
                    if (TempPoints != null)
                    {
                        points += Convert.ToInt32(TempPoints);
                    }
                }
                MyDict.Add(division.Name, points);
            }

            return MyDict;
        }
    }
}