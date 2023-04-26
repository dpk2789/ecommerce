

using System.ComponentModel.DataAnnotations;
using TreeUtility;

namespace Ecommerce.Infrastructure.Domains
{
    public class ProductCategory :  ITreeNode<ProductCategory>
    {
        public Guid Id { get; set; }
        private Guid? _parentCategoryId;

        [Display(Name = "Parent Category")]
        public Guid? ParentCategoryId
        {
            get { return _parentCategoryId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");

                _parentCategoryId = value;
            }
        }

        public string? Name { get; set; }
        public string? Type { get; set; }
        public virtual ProductCategory Parent { get; set; }
        public IList<ProductCategory> Children { get; set; }
        public virtual IList<Product> Products { get; set; }      
    }
}
