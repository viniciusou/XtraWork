namespace XtraWork.API.Entities
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string TitleDescription { get; set; }
        public Guid TitleId { get; set; }
    }
}