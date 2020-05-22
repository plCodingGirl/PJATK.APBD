using System;

namespace CW2.Models
{
    public class UserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}