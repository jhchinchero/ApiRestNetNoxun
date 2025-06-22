using System.Collections.Generic;

namespace ApiRestNetNoxun.Dtos
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
