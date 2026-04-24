using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.UserModel
{
    public class UsersModel
    {

        [Key]
        public int UserID { get; set; }

        public string UserName { get; set; }
        public string UserLastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }

        private UsersModel() { }

        public static UsersModel Create(string userName, string userLastName, string passwordHash, string email, string phoneNumber)
        {
            return new UsersModel
            {
                UserName = userName,
                UserLastname = userLastName,
                PasswordHash = passwordHash,
                Email = email,
                PhoneNumber = phoneNumber
            };
        }
    }
}
