using FullStackAssessment_U3Infotech.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssessment_U3Infotech.Application.Commands.CafeCommands
{ 
    public class DeleteCafeCommandHandler : IRequestHandler<DeleteCafeCommand, bool>
    {
        private readonly AssessmentDbContext _context;

        public DeleteCafeCommandHandler(AssessmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCafeCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cafe = await _context.Cafes.Include(c => c.EmployeeCafeRelations)
                .FirstOrDefaultAsync(c => c.ID == request.Id); 

                if (cafe == null)
                {
                    return false;
                }
                //Delete employees and relations associated with cafe
                if (cafe.EmployeeCafeRelations.Count > 0)
                {
                    foreach (var ecr in cafe.EmployeeCafeRelations)
                    {
                        var employee = _context.Employees.Find(ecr.EmployeeID);
                        if (employee != null)
                        {
                            _context.Employees.Remove(employee);
                        }
                    }
                    _context.EmployeeCafeRelations.RemoveRange(cafe.EmployeeCafeRelations);
                }
                _context.Cafes.Remove(cafe);
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
    public class DeleteCafeCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteCafeCommand(int id)
        {
            Id = id;
        }
    }
}
