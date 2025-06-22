// RoleDto.cs
namespace ApiRestNetNoxun.Dtos
{
    public class RoleDto
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}

// UserRoleDto.cs
namespace ApiRestNetNoxun.Dtos
{
    public class UserRoleDto
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}
