namespace XtraWork.API.Requests
{
    public class EmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public Guid TitleId { get; set; }
    }
}