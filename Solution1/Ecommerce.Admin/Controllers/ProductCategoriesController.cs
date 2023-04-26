using Ecommerce.Admin.ViewModels;
using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Admin.Controllers
{
    public class ProductCategoriesController : Controller
    {
        public readonly IProductCategoryRepository _productCategoryRepository;
        public ProductCategoriesController(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }
        public async Task<ActionResult<IList<ProductCategoryViewModel>>> Index()
        {
            var data = await _productCategoryRepository.FindAll().Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
            }).OrderBy(ow => ow.Name).ToListAsync();

            return View(data);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCategoryViewModel viewModel)
        {
            try
            {
                ProductCategory productCategory = new ProductCategory();
                productCategory.Id = Guid.NewGuid();
                productCategory.Name = viewModel.Name;
                _productCategoryRepository.Create(productCategory);
                await _productCategoryRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var productCateogory = await _productCategoryRepository.FindByCondition(x => x.Id == id).FirstOrDefaultAsync();
            if (productCateogory == null)
            {
                return View();
            }
            ProductCategoryViewModel productCategoryViewModel = new ProductCategoryViewModel();
            productCategoryViewModel.Id = id;
            productCategoryViewModel.Name = productCateogory.Name;
            return View(productCategoryViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, ProductCategoryViewModel productCategoryViewModel)
        {
            try
            {
                if (id != productCategoryViewModel.Id)
                {
                    return View();
                }
                var productCategory = await _productCategoryRepository.FindByCondition(x => x.Id == productCategoryViewModel.Id).FirstOrDefaultAsync();
                if (productCategory == null)
                {
                    return View();
                }
                productCategory.Name = productCategoryViewModel.Name;
                _productCategoryRepository.Update(productCategory);
                await _productCategoryRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            var productCateogory = await _productCategoryRepository.FindByCondition(x => x.Id == id).FirstOrDefaultAsync();
            if (productCateogory == null)
            {
                return View();
            }
            ProductCategoryViewModel productCategoryViewModel = new ProductCategoryViewModel();
            productCategoryViewModel.Id = id;
            productCategoryViewModel.Name = productCateogory.Name;
            return View(productCategoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ProductCategoryViewModel productCategoryViewModel)
        {
            try
            {
                var productCategory = await _productCategoryRepository.FindByCondition(x => x.Id == productCategoryViewModel.Id).FirstOrDefaultAsync();
                if (productCategory == null)
                {
                    return View();
                }
                _productCategoryRepository.Delete(productCategory);
                await _productCategoryRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
