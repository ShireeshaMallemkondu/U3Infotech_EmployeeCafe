using FullStackAssessment_U3Infotech.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssessment_U3Infotech.Application.Commands.EmployeeCommands
{ 
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly AssessmentDbContext _context;

        public DeleteEmployeeCommandHandler(AssessmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = await _context.Employees.Include(c => c.EmployeeCafeRelations)
                .FirstOrDefaultAsync(c => c.ID == request.Id); ;

                if (employee == null)
                {
                    return false;
                }
                _context.EmployeeCafeRelations.RemoveRange(employee.EmployeeCafeRelations);

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
            
        }
    }
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public DeleteEmployeeCommand(string id)
        {
            Id = id;
        }
    }
}
