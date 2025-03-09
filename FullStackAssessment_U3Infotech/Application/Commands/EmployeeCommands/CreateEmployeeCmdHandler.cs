using FullStackAssessment_U3Infotech.Data;
using FullStackAssessment_U3Infotech.Models;
using MediatR;

namespace FullStackAssessment_U3Infotech.Application.Commands.EmployeeCommands
{
    public class CreateEmployeeCmdHandler : IRequestHandler<CreateEmployeeRequest, EmployeeRes>
    {
        private readonly AssessmentDbContext _context;
        public CreateEmployeeCmdHandler(AssessmentDbContext context)
        {
            _context = context;
        }
        public async Task<EmployeeRes> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var newEmployee = new Employee
            {
                ID = $"UI{Guid.NewGuid().ToString("N").ToUpper()}", 
                Name = request.Name,
                Email_Address = request.Email_Address,
                Phonenumber = request.Phonenumber,
                Gender = request.Gender
            };

            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync(cancellationToken);

            var employeeCafeRelation = new EmployeeCafeRelation
            {
                EmployeeID = newEmployee.ID,
                CafeID = request.CafeID,
                StartDate = DateTime.Now
            };

            _context.EmployeeCafeRelations.Add(employeeCafeRelation);
            await _context.SaveChangesAsync(cancellationToken);

            var cafe =await _context.Cafes.FindAsync(request.CafeID);
            var employeeResponse = new EmployeeRes
            {
                ID = newEmployee.ID,
                Name = newEmployee.Name,
                Emailaddress = newEmployee.Email_Address,
                Phonenumber = newEmployee.Phonenumber,
                Cafe = cafe?.Name ?? string.Empty
            };

            return employeeResponse;
        }
    }

    public class CreateEmployeeRequest : IRequest<EmployeeRes>
    {
        public string Name { get; set; }
        public string Email_Address { get; set; }
        public string Phonenumber { get; set; }
        public string Gender { get; set; }
        public int CafeID { get; set; }
    }

    public class EmployeeRes
    {
        public string ID { get; set; } 
        public string Name { get; set; }
        public string Emailaddress { get; set; }
        public string Phonenumber { get; set; }
        public string Cafe { get; set; }
    }
}
