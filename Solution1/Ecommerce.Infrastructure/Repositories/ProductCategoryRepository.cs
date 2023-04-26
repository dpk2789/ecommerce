

using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(EcommerceDbContext repositoryContext)
        : base(repositoryContext)
        {
        }
    }
}
