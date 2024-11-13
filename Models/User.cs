namespace Steam.Models
{
    public class User
    {
       

        int id;
        string name;
        string email;
        string password;
        //קונסטרקטור
        public User(int id, string name, string email, string password)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.password = password;
        }
        //קונסטרקטור ריק
        public User() { }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email1 { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }

        static public List<User> Userslist = new List<User>();


        public bool Insert()
        {
            try
            {
                if (Userslist.Any(item => item.Id == this.Id || item.name == this.name))
                {
                    return false;
                }
                Userslist.Add(this);
                return true;
            }
            catch
            {
                return false;
            }
           
        }
        public static List<User> read()
        {

            return Userslist;
        }
    }
}
