using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserHubAPI.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }

        public Users User { get; set; } = null!;

        public Guid RoleId { get; set; }

        public Roles Role { get; set; } = null!;
    }
}