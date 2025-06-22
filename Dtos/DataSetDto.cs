namespace ApiRestNetNoxun.Dtos
{
    public class DataSetDto
    {
        public int DataSetID { get; set; }
        public string DataSetName { get; set; }
        public string Description { get; set; }

        public int ProcedureID { get; set; }
        public int FieldID { get; set; }

        public string FieldName { get; set; }
        public string DataType { get; set; }
    }
}
