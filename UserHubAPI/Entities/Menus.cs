using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserHubAPI.Entities
{
    public class Menus : Base
    {
        [Required(ErrorMessage = "Menu name is required")]
        private string _menuName = null!;

        [Required(ErrorMessage = "Menu description is required")]
        private string _menuDescription = null!;

        public String MenuName { get => _menuName; set => _menuName = value; }

        public String MenuDescription { get => _menuDescription; set => _menuDescription = value; }

        public Guid ParentId { get; set; } = Guid.Empty;

        public String? ControllerName { get; set; }

        public String? ActionName { get; set; }

        public ICollection<RoleMenu> RoleMenu { get; set; } = null!;
    }
}