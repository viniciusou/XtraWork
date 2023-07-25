namespace XtraWork.API.Responses
{
    public class EmployeeResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string TitleDescription { get; set; }
        public Guid TitleId { get; set; }
    }
}