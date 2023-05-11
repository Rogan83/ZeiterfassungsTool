namespace ZeiterfassungsTool.MySQLModels
{
    public class Employee
    {
        public int Id { get; set; } = 1;
        public string Username { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime Birthday { get; set; } = DateTime.Now;
        public string EMail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public double WorkingHoursPerWeek { get; set; } = 0;
        public int VacationDaysPerYear { get; set; } = 30;             //Legt die Urlaubstage fest, die dem Mitarbeiter pro Jahr zur Verfügung stehen.
        public bool IsResetPassword { get; set; } = false;
        public int RoleId { get; set; } = 1;

    }
}
