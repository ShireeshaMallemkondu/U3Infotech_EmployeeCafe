namespace FullStackAssessment_U3Infotech.Models
{
    public class Employee
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email_Address { get; set; }
        public string Phonenumber { get; set; }
        public string Gender { get; set; }


        public List<EmployeeCafeRelation> EmployeeCafeRelations { get; set; }
    }
}
