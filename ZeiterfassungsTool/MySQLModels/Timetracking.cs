namespace ZeiterfassungsTool.MySQLModels
{
    public class Timetracking
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Subject { get; set; } = string.Empty;
        public int Typeid { get; set; }
        public int EmployeeId { get; set; }
    }
}
