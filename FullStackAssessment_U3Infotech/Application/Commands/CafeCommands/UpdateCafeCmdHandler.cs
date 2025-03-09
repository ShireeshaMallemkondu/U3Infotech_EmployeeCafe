using FullStackAssessment_U3Infotech.Data;
using FullStackAssessment_U3Infotech.Models;
using MediatR;

namespace FullStackAssessment_U3Infotech.Application.Commands.CafeCommands
{
    public class UpdateCafeCmdHandler : IRequestHandler<UpdatedCafe, bool>
    {
        private readonly AssessmentDbContext _context;
        public UpdateCafeCmdHandler(AssessmentDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdatedCafe request, CancellationToken cancellationToken)
        {
            Cafe cafe = await _context.Cafes.FindAsync(request.Id);

            if (cafe == null)
            {
                return false;
            }

            cafe.Name = request.Name;
            cafe.Location = request.Location;
            cafe.Description = request.Description;

            await _context.SaveChangesAsync();
            return true;
        }
    }

    

    public class UpdatedCafe(int id, string name, string location, string description) : IRequest<bool>
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Location { get; set; } = location;
        public string Description { get; set; } = description;
    }

    
}
