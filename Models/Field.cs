using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ApiRestNetNoxun.Models{


public class Field
{
    [Key]
    public int FieldID { get; set; }

    [Required]
    public string FieldName { get; set; }

    [Required]
    public string DataType { get; set; }

    public ICollection<DataSet> DataSets { get; set; }
}
}