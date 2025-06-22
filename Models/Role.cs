using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiRestNetNoxun.Models{
public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string? Description { get; set; }

        // Navegación
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}