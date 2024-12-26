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
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }




  
    }
}
