

using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        protected readonly EcommerceDbContext _context;

        public ProductRepository(EcommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
