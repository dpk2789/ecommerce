

using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CartRepository : RepositoryBase<CartProduct>, ICartRepository
    {
        protected readonly EcommerceDbContext _context;

        public CartRepository(EcommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
