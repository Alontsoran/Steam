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
        private bool isActive;
        private Double Number;

        public User(string id, string name, string email, string password, bool isActive = true, Double number = 0)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            IsActive = isActive;
            Number1 = number;
        }
       
        public User() { }

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public Double Number1 { get => Number; set => Number = value; }
    }
}