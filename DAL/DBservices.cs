using Steam.Models;
using System.Data.SqlClient;

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
    }
}
