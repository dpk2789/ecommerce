

using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        protected readonly EcommerceDbContext _context;

        public ProductCategoryRepository(EcommerceDbContext context) : base(context)
        {
            _context = context;
        }
        //public ProductCategoryRepository(EcommerceDbContext repositoryContext)
        //: base(repositoryContext)
        //{
        //}
    }
}
