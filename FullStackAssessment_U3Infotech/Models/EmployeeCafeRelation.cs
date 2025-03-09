namespace FullStackAssessment_U3Infotech.Models
{
    public class EmployeeCafeRelation
    {
        public int ID { get; set; }
        public int CafeID { get; set; }
        public Cafe Cafe { get; set; }
        public string EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
       
    }
}
