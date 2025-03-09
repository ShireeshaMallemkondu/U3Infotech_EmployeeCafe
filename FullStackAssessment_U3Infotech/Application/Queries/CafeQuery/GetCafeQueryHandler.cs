using FullStackAssessment_U3Infotech.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssessment_U3Infotech.Application.Queries.CafeQuery
{
    public class GetCafeQueryHandler: IRequestHandler<GetCafesQuery, List<CafeRes>>
    {
        private readonly AssessmentDbContext _context;
        public GetCafeQueryHandler(AssessmentDbContext context)
        {
            _context = context;
        }

        public Task<List<CafeRes>> Handle(GetCafesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Cafes.AsQueryable();

            if (!string.IsNullOrEmpty(request.Location))
            {
                query = query.Where(c => c.Location == request.Location);
            }

            var cafes =  query
                .Select(c => new CafeRes
                {
                    ID = c.ID,
                    Name = c.Name,
                    Description = c.Description,
                    Location = c.Location,
                    Employees = c.EmployeeCafeRelations.Count
                })
                .OrderByDescending(c => c.Employees)
                .ToListAsync();

            return cafes;
        }
    }

    public class GetCafesQuery : IRequest<List<CafeRes>>
    {
        public string Location { get; set; }
    }

    public class CafeRes
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Employees { get; set; }
        public string Location { get; set; }
    }
}
