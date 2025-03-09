using FullStackAssessment_U3Infotech.Data;
using FullStackAssessment_U3Infotech.Models;
using MediatR;

namespace FullStackAssessment_U3Infotech.Application.Commands.CafeCommands
{
    public class CreateCafeCmdHandler :IRequestHandler<CreateCafeCmd, Cafe>
    {
        private readonly AssessmentDbContext _context;
        public CreateCafeCmdHandler(AssessmentDbContext context)
        {
            _context = context;
        }

        public async Task<Cafe> Handle(CreateCafeCmd request, CancellationToken cancellationToken)
        {
            var cafe = new Cafe
            {
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                Logo = request.Logo
            };
            _context.Cafes.Add(cafe);
            await _context.SaveChangesAsync();

            return cafe;
        }
    }

    public class CreateCafeCmd:IRequest<Cafe>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
    }
}
