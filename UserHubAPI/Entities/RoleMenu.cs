using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserHubAPI.Entities
{
    public class RoleMenu
    {
        public Guid RoleId { get; set; }

        public Roles Role { get; set; } = null!;

        public Guid MenuId { get; set; }

        public Menus Menu { get; set; } = null!;
    }
}