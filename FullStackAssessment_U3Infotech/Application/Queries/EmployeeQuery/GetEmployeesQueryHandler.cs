using FullStackAssessment_U3Infotech.Data;
using FullStackAssessment_U3Infotech.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssessment_U3Infotech.Application.Queries.EmployeeQuery
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeResponse>>
    {
        private readonly AssessmentDbContext _context;
        public GetEmployeesQueryHandler(AssessmentDbContext context)
        {
            _context = context;
        }
        public Task<List<EmployeeResponse>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<EmployeeCafeRelation> employeeCafeRelations = _context.EmployeeCafeRelations
            .Include(ecr => ecr.Employee)
            .Include(ecr => ecr.Cafe);

            if (request.CafeId != null)
            {
                employeeCafeRelations = employeeCafeRelations.Where(ecr => ecr.Cafe.ID.Equals(request.CafeId));
            }

            var employees =  employeeCafeRelations
                .Select(ecr => new EmployeeResponse
                {
                    EmployeeID = ecr.Employee.ID,
                    Name = ecr.Employee.Name,
                    EmailAddress = ecr.Employee.Email_Address,
                    PhoneNumber = ecr.Employee.Phonenumber,
                    Gender = ecr.Employee.Gender,
                    DaysWorked = EF.Functions.DateDiffDay(ecr.StartDate, DateTime.Now),
                    CafeID = ecr.Cafe.ID,
                    Cafe=ecr.Cafe.Name
                })
                .OrderByDescending(e => e.DaysWorked) 
                .ToListAsync(cancellationToken);

            return employees;
        }
    }


    public class GetEmployeesQuery : IRequest<List<EmployeeResponse>>
    {
        public int? CafeId { get; set; }

    }

    public class EmployeeResponse
    {
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }  
        public int DaysWorked { get; set; }
        public int CafeID { get; set; }
        public string Cafe { get; set; }
    }
}
