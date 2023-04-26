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
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                ProductCategory productCategory = new ProductCategory();
                _productCategoryRepository.Create(productCategory);
                _productCategoryRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
       
        public ActionResult Edit(int id)
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
       
        public ActionResult Delete(int id)
        {
            return View();
        }
               
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
