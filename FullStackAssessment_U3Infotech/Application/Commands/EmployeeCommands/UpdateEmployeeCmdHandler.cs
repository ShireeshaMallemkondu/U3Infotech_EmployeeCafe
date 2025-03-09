using FullStackAssessment_U3Infotech.Data;
using FullStackAssessment_U3Infotech.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssessment_U3Infotech.Application.Commands.EmployeeCommands
{
    public class UpdateEmployeeCmdHandler : IRequestHandler<UpdatedEmployee, bool>
    {
        private readonly AssessmentDbContext _context;
        public UpdateEmployeeCmdHandler(AssessmentDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdatedEmployee request, CancellationToken cancellationToken)
        {
            Employee employee = await _context.Employees.FindAsync(request.EmployeeID);

            if (employee == null)
            {
                return false;
            }

            employee.Name = request.Name;
            employee.Email_Address = request.Email_Address;
            employee.Phonenumber = request.Phonenumber;
            employee.Gender = request.Gender;

            // Find the existing EmployeeCafeRelation
            var employeeCafeRelation = await _context.EmployeeCafeRelations
                .FirstOrDefaultAsync(ecr => ecr.EmployeeID == request.EmployeeID, cancellationToken);

            if (employeeCafeRelation != null)
            {
                // Update the CafeID
                employeeCafeRelation.CafeID = request.CafeID;
            }
            else
            {
                // If no existing relation, create a new one
                employeeCafeRelation = new EmployeeCafeRelation
                {
                    EmployeeID = request.EmployeeID,
                    CafeID = request.CafeID,
                    StartDate = DateTime.Now 
                };
                _context.EmployeeCafeRelations.Add(employeeCafeRelation);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }



    public class UpdatedEmployee : IRequest<bool>
    {
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email_Address { get; set; }
        public string Phonenumber { get; set; }
        public string Gender { get; set; }
        public int CafeID { get; set; }

        public UpdatedEmployee(string employeeID, string name, string email_Address, string phonenumber, string gender, int cafeID)
        {
            EmployeeID = employeeID;
            Name = name;
            Email_Address = email_Address;
            Phonenumber = phonenumber;
            Gender = gender;
            CafeID = cafeID;
        }
    }


}
