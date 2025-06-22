using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRestNetNoxun.Models
{
    public class DataSet
{
    [Key]
    public int DataSetID { get; set; }

    [Required]
    public string DataSetName { get; set; }

    public string Description { get; set; }

    [ForeignKey("Procedure")]
    public int ProcedureID { get; set; }
    public Procedure Procedure { get; set; }

    [ForeignKey("Field")]
    public int FieldID { get; set; }
    public Field Field { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }
}

}
