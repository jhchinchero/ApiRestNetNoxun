using ApiRestNetNoxun.Dtos;

namespace ApiRestNetNoxun.Dtos
{
    public class LoginResponseDto
{
    public List<UserDto> Users { get; set; }
    public List<RoleDto> Roles { get; set; }
    public List<UserRoleDto> UserRoles { get; set; }
    public List<ProcedureDto> Procedures { get; set; }
    public List<FieldDto> Fields { get; set; }
    public List<DataSetDto> DataSets { get; set; }
}

}