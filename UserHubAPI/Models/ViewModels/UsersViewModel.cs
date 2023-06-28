using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UserHubAPI.Entities;

namespace UserHubAPI.Models
{
    public class UsersViewModel : Base
    {
        [Required(ErrorMessage = "Username is required")]
        private string _userName = null!;

        [Required(ErrorMessage = "Password is required")]
        private string _password = null!;

        [Required(ErrorMessage = "LoginID is required")]
        private string _loginID = null!;

        [Required(ErrorMessage = "Email is required")]
        private string _email = null!;

        public String UserName { get => _userName; set => _userName = value; }

        public String Password { get => _password; set => _password = value; }

        public String LoginID { get => _loginID; set => _loginID = value; }

        public String Email { get => _email; set => _email = value; }

        public ICollection<Guid> Roles { get; set; } = null!;
    }
}