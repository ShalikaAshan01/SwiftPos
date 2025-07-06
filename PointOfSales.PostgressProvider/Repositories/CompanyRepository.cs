using PointOfSales.Core.Entities.Infrastructure;
using PointOfSales.Core.IRepositories;

namespace PointOfSales.PostgressProvider.Repositories;

public class CompanyRepository: GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(MyDbContext context) : base(context)
    {
    }
}