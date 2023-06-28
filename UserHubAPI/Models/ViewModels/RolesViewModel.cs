using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UserHubAPI.Entities;

namespace UserHubAPI.Models
{
    public class RolesViewModel : Base
    {
        [Required(ErrorMessage = "Role name is required")]
        private string _roleName = null!;

        [Required(ErrorMessage = "Role description is required")]
        private string _roleDescription = null!;

        public String RoleName { get => _roleName; set => _roleName = value; }

        public String RoleDescription { get => _roleDescription; set => _roleDescription = value; }

        public ICollection<Guid> Menus { get; set; } = null!;
    }
}