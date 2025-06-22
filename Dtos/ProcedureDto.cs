using System.Collections.Generic;

namespace ApiRestNetNoxun.Dtos
{
    public class ProcedureDto
    {
        public int ProcedureID { get; set; }
        public string ProcedureName { get; set; }
        public string Description { get; set; }
        public int CreatedByUserID { get; set; }
        public int? LastModifiedUserID { get; set; }

        public List<DataSetDto> DataSets { get; set; }
    }
}
