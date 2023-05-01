

using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductImageRepository : RepositoryBase<ProductImage>, IProductImageRepository
    {
        protected readonly EcommerceDbContext _context;
        public ProductImageRepository(EcommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
