using Steam.DAL;
using System.Collections.Generic;

namespace Steam.Models
{
    public class User
    {
        private string id;
        private string name;
        private string email;
        private string password;

        // Constructor
        public User(string id, string name, string email, string password)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.password = password;
        }

        // Empty constructor
        public User() { }

        // Properties
        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }

      
    }
}