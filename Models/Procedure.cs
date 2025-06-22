using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRestNetNoxun.Models{
public class Procedure
{
    [Key]
    public int ProcedureID { get; set; }

    [Required]
    public string ProcedureName { get; set; }

    public string Description { get; set; }

    [ForeignKey("CreatedByUser")]
    public int CreatedByUserID { get; set; }
    public User CreatedByUser { get; set; }

    public DateTime CreatedDate { get; set; }

    [ForeignKey("LastModifiedUser")]
    public int? LastModifiedUserID { get; set; }
    public User LastModifiedUser { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public ICollection<DataSet> DataSets { get; set; }
}
}