using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRestNetNoxun.Models{
 public class UserRole
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }
        public int RoleID { get; set; }

        // Navegación
        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}