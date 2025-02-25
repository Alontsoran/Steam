﻿using Microsoft.Extensions.Logging;
using Steam.Models;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Steam.Models;  // הוסף את זה בראש הקובץ

namespace Steam.DAL
{
    public class DBservices
    {
        public DBservices()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

            return cmd;
        
        
        
        }
        //כל מה שקשור למשתמשים
        public int Register(User user)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@Email", user.Email },
            { "@Name", user.Name },
            { "@Password", user.Password }
        };

            using (SqlConnection con = connect("myProjDB"))
            {
                using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("AddNewUser", con, paramDic))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public User Login(string email, string password)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>
    {
        { "@Email", email },
        { "@Password", password }
    };

            using (SqlConnection con = connect("myProjDB"))
            {
                using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("LoginUser", con, paramDic))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User();
                            user.Email = reader["Email"].ToString();  // מעתיק את האימייל מה-DB
                            user.Name = reader["Name"].ToString();    // מעתיק את השם מה-DB
                            return user;
                        }

                        return null;
                    }
                }
            }
        }
        //הוספת משחקל DB
        public int AddGame(string Email, string game)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>
    {
        { "@Email", Email },
        { "@GameId", game }
    };

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("AddGameToUser", con, paramDic))
            {
                try
                {
                    // הוספת פרמטר לקבלת ערך החזרה
                    SqlParameter returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    // קבלת ערך החזרה מה-SP
                    int result = (int)returnParameter.Value;
                    return result;  // יחזיר 0 אם המשחק קיים, 1 אם נוסף בהצלחה
                }
                catch (Exception ex)
                {
                    // לוג של השגיאה אם יש
                    Console.WriteLine($"Error in AddGame: {ex.Message}");
                    throw;  // או להחזיר -1 במקרה של שגיאה
                }
            }
        }
        public int DeleteGame(string Email, string game)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>
    {
        { "@Email", Email },
        { "@GameId", game }
    };

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("DeleteGameToUser", con, paramDic))
            {
                try
                {
                    // הוספת פרמטר לקבלת ערך החזרה
                    SqlParameter returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    // קבלת ערך החזרה מה-SP
                    int result = (int)returnParameter.Value;
                    return result;  // יחזיר 0 אם המשחק קיים, 1 אם נוסף בהצלחה
                }
                catch (Exception ex)
                {
                    // לוג של השגיאה אם יש
                    Console.WriteLine($"Error in AddGame: {ex.Message}");
                    throw;  // או להחזיר -1 במקרה של שגיאה
                }
            }
        }
        public List<Game> ShowUserGames(string Email)
        {
            List<Game> games = new List<Game>();
            Dictionary<string, object> paramDic = new Dictionary<string, object>
    {
        { "@Email", Email }
    };

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("ListUserGames", con, paramDic))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Game game = new Game(
                        appId1: Convert.ToInt32(reader["AppID"]),
                        name1: reader["Name"].ToString(),
                        releaseDate1: DateTime.Parse(reader["Release_date"].ToString()),
                        price1: Convert.ToDecimal(reader["Price"]),
                        description: reader["description"].ToString(),
                        headerImage1: reader["Header_image"].ToString(),
                        website1: reader["Website"].ToString(),
                        windows1: Convert.ToBoolean(reader["Windows"]),
                        mac1: Convert.ToBoolean(reader["Mac"]),
                        linux1: Convert.ToBoolean(reader["Linux"]),
                        scoreRank1: Convert.ToInt32(reader["Score_rank"]),
                        recommendations: reader["Recommendations"].ToString(),
                        publisher1: reader["Developers"].ToString(),
                                                numberOfPurchases: Convert.ToInt32(reader["numberOfPurchases"])

                    );
                    games.Add(game);
                }
            }
            return games;
        }
        public List<Game> ShowUserWithPriceGames(string Email,float price)
        {
            List<Game> games = new List<Game>();
            Dictionary<string, object> paramDic = new Dictionary<string, object>
    {
        { "@Email", Email },
        {"@price",price }
    };

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("ListPriceUserGames", con, paramDic))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Game game = new Game(
                        appId1: Convert.ToInt32(reader["AppID"]),
                        name1: reader["Name"].ToString(),
                        releaseDate1: DateTime.Parse(reader["Release_date"].ToString()),
                        price1: Convert.ToDecimal(reader["Price"]),
                        description: reader["description"].ToString(),
                        headerImage1: reader["Header_image"].ToString(),
                        website1: reader["Website"].ToString(),
                        windows1: Convert.ToBoolean(reader["Windows"]),
                        mac1: Convert.ToBoolean(reader["Mac"]),
                        linux1: Convert.ToBoolean(reader["Linux"]),
                        scoreRank1: Convert.ToInt32(reader["Score_rank"]),
                        recommendations: reader["Recommendations"].ToString(),
                        publisher1: reader["Developers"].ToString(),
                                                numberOfPurchases: Convert.ToInt32(reader["numberOfPurchases"])

                    );
                    games.Add(game);
                }
            }
            return games;
        }

        public List<Game> showGamesWIthRank(string Email, float Rank)
        {
            List<Game> games = new List<Game>();
            Dictionary<string, object> paramDic = new Dictionary<string, object>
    {
        { "@Email", Email },
        {"@Rank",Rank }
    };

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("ListRankUserGames", con, paramDic))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Game game = new Game(
                        appId1: Convert.ToInt32(reader["AppID"]),
                        name1: reader["Name"].ToString(),
                        releaseDate1: DateTime.Parse(reader["Release_date"].ToString()),
                        price1: Convert.ToDecimal(reader["Price"]),
                        description: reader["description"].ToString(),
                        headerImage1: reader["Header_image"].ToString(),
                        website1: reader["Website"].ToString(),
                        windows1: Convert.ToBoolean(reader["Windows"]),
                        mac1: Convert.ToBoolean(reader["Mac"]),
                        linux1: Convert.ToBoolean(reader["Linux"]),
                        scoreRank1: Convert.ToInt32(reader["Score_rank"]),
                        recommendations: reader["Recommendations"].ToString(),
                        publisher1: reader["Developers"].ToString(),
                        numberOfPurchases: Convert.ToInt32(reader["numberOfPurchases"])
                    );
                    games.Add(game);
                }
            }
            return games;
        }
        public List<Game> GetAllGames()
        {
            List<Game> games = new List<Game>();
            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GetAllGames", con, paramDic))
            {
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            games.Add(new Game(
                                appId1: Convert.ToInt32(reader["AppID"]),
                                name1: reader["Name"].ToString(),
                                releaseDate1: DateTime.Parse(reader["Release_date"].ToString()),
                                price1: Convert.ToDecimal(reader["Price"]),
                                description: reader["description"].ToString(),
                                headerImage1: reader["Header_image"].ToString(),
                                website1: reader["Website"].ToString(),
                                windows1: Convert.ToBoolean(reader["Windows"]),
                                mac1: Convert.ToBoolean(reader["Mac"]),
                                linux1: Convert.ToBoolean(reader["Linux"]),
                                scoreRank1: Convert.ToInt32(reader["Score_rank"]),
                                recommendations: reader["Recommendations"].ToString(),
                                publisher1: reader["Developers"].ToString(),
                                                        numberOfPurchases: Convert.ToInt32(reader["numberOfPurchases"])

                            ));
                        }
                    }
                    return games;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetAllGames: {ex.Message}");
                    throw;
                }
            }
        }
        public int UpdateUser(User user, string newPassword)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

        
            if (string.IsNullOrEmpty(user.Id.ToString()))
                throw new ArgumentNullException("Id");

            var paramDic = new Dictionary<string, object>
    {
        { "@Id", user.Id },                          // מזהה המשתמש
        { "@Newemail", user.Email ?? "" },           // אימייל חדש
        { "@Newname", user.Name ?? "" },             // שם חדש
        { "@NewPassword", newPassword ?? "" }        // סיסמה חדשה (רק אם יש)
    };

            using (SqlConnection con = connect("myProjDB"))
            {
                if (con == null)
                    throw new Exception("Database connection failed");

                using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_UpdateUser", con, paramDic))
                {
                    try
                    {
                        // כי ב-SP אנחנו עושים SELECT 1 או SELECT 0
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int numericResult))
                        {
                            return numericResult; // 1 (הצליח) או 0 (נכשל)
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to update user", ex);
                    }
                }
            }
        }


        public string GetUserId(string email)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>
   {
       { "@email", email }
       
   };
            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("GetUserIdAfterLogin", con, paramDic))
            {
                try
                {
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "-1";
                }
                catch
                {
                    return "-1";
                }
            }
        }

        public User GetUserData(string userId)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>
    {
        { "@UserId", Guid.Parse(userId) }  // Converting string to GUID
    };

            using (SqlConnection con = connect("myProjDB"))
            {
                using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GetUserData", con, paramDic))
                {
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User(
                                    id: reader["UserId"].ToString(),
                                    name: reader["Name"].ToString(),
                                    email: reader["Email"].ToString(),
                                    password: reader["Password"].ToString(),
                                    isActive: reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false
                                );
                            }
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in GetUserData: {ex.Message}");
                        throw;
                    }
                }
            }
        }
        public List<User> GetFilteredUsers(bool? isActive = null, string searchTerm = "")
        {
            List<User> users = new List<User>();
            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            if (isActive.HasValue)
            {
                paramDic.Add("@IsActive", isActive.Value);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                paramDic.Add("@SearchTerm", searchTerm);
            }

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GetFilteredUsers", con, paramDic))
            {
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User(
                                id: reader["UserId"].ToString(),
                                name: reader["Name"].ToString(),
                                email: reader["Email"].ToString(),
                                password: reader["Password"].ToString(),
                                isActive: reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false
                            ));
                        }
                    }
                    return users;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetFilteredUsers: {ex.Message}");
                    throw;
                }
            }
        }
        public List<Game> GetFilteredGames(decimal? maxPrice, int? minScore, string platform, string searchTerm)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            if (maxPrice.HasValue)
                paramDic.Add("@MaxPrice", maxPrice.Value);
            if (minScore.HasValue)
                paramDic.Add("@MinScore", minScore.Value);
            if (!string.IsNullOrEmpty(platform))
                paramDic.Add("@Platform", platform);
            if (!string.IsNullOrEmpty(searchTerm))
                paramDic.Add("@SearchTerm", searchTerm);

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GetFilteredGames", con, paramDic))
            {
                List<Game> games = new List<Game>();

                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            games.Add(new Game(
                                appId1: Convert.ToInt32(reader["AppID"]),
                                name1: reader["Name"].ToString(),
                                releaseDate1: DateTime.Parse(reader["Release_date"].ToString()),
                                price1: Convert.ToDecimal(reader["Price"]),
                                description: reader["description"].ToString(),
                                headerImage1: reader["Header_image"].ToString(),
                                website1: reader["Website"].ToString(),
                                windows1: Convert.ToBoolean(reader["Windows"]),
                                mac1: Convert.ToBoolean(reader["Mac"]),
                                linux1: Convert.ToBoolean(reader["Linux"]),
                                scoreRank1: Convert.ToInt32(reader["Score_rank"]),
                                recommendations: reader["Recommendations"].ToString(),
                                publisher1: reader["Developers"].ToString(),
                                                        numberOfPurchases: Convert.ToInt32(reader["numberOfPurchases"])

                            ));
                        }
                    }
                    return games;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetFilteredGames: {ex.Message}");
                    throw;
                }
            }
        }


        // Get all users
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GetAllUsers", con, paramDic))
            {
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User(
                                id: reader["UserId"].ToString(),
                                name: reader["Name"].ToString(),
                                email: reader["Email"].ToString(),
                                password: reader["Password"].ToString(),
                                isActive: reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false,
                                number: Convert.ToDouble(reader["NumOfp"])

                            ));
                        }
                    }
                    return users;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetAllUsers: {ex.Message}");
                    throw;
                }
            }
        }

        // Get active users only
        public List<User> GetActiveUsers()
    {
        List<User> users = new List<User>();
        Dictionary<string, object> paramDic = new Dictionary<string, object>();

        using (SqlConnection con = connect("myProjDB"))
        using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GetActiveUsers", con, paramDic))
        {
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User(
                            id: reader["UserId"].ToString(),
                            name: reader["Name"].ToString(),
                            email: reader["Email"].ToString(),
                            password: reader["Password"].ToString(),
                            isActive: true
                        ));
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetActiveUsers: {ex.Message}");
                throw;
            }
        }
    }

    // Get user by email
    public User GetUserByEmail(string email)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@Email", email }
        };

        using (SqlConnection con = connect("myProjDB"))
        using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GetUserByEmail", con, paramDic))
        {
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User(
                            id: reader["UserId"].ToString(),
                            name: reader["Name"].ToString(),
                            email: reader["Email"].ToString(),
                            password: reader["Password"].ToString(),
                            isActive: reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false
                        );
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByEmail: {ex.Message}");
                throw;
            }
        }

    }
        public bool UpdateUserStatus(string email, bool isActive)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>
    {
        { "@Email", email },
        { "@IsActive", isActive ? 1 : 0 }
    };

            using (SqlConnection con = connect("myProjDB"))
            using (SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_UpdateUserStatus", con, paramDic))
            {
                try
                {
                    int result = (int)cmd.ExecuteScalar();
                    return result == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating user status: {ex.Message}");
                    return false;
                }
            }
        }

    }


}
