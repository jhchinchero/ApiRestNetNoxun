using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ApiRestNetNoxun.Models{
 public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        // Navegación
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<Procedure>? ProceduresCreated { get; set; }
        public ICollection<Procedure>? ProceduresModified { get; set; }
    }
}