using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackAssessment_U3Infotech.Models
{
    public class Cafe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
        public List<EmployeeCafeRelation> EmployeeCafeRelations { get; set; }
    }
}
