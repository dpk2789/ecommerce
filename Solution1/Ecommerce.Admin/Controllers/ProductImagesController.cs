using Ecommerce.Admin.ViewModels;
using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        public readonly IProductRepository _productRepository;
        public readonly IProductImageRepository _productImageRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductImagesController(IProductRepository productRepository, IProductImageRepository productImageRepository, IWebHostEnvironment hostEnvironment)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            this._hostEnvironment = hostEnvironment;
        }
        public async Task<ActionResult<IList<ProductImageViewModel>>> Index(Guid productId)
        {
            var product = await _productRepository.FindByCondition(x => x.Id == productId).FirstOrDefaultAsync();
            var data = await _productImageRepository.FindByCondition(x => x.ProductId == productId).Include(x => x.Product).Select(x => new ProductImageViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ProductName = x.Product.Name,
                RelativePath = x.RelativePath,
                Extention = x.Extention
            }).OrderBy(ow => ow.Name).ToListAsync();
            ViewData["ProductName"] = product.Name;
            ViewData["ProductId"] = product.Id;
            return View(data);
        }

        public async Task<ActionResult> Create(Guid productId)
        {
            ProductImageViewModel imageModel = new ProductImageViewModel();
            imageModel.ProductId = productId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductImageViewModel imageModel)
        {
            if (ModelState.IsValid)
            {
                if (imageModel.ImageFile == null || imageModel.ImageFile.Length == 0)
                {
                    return Content("File not selected");
                }
                string uniqueFileName = null;

                long maxFileSize = 1024 * 1024;
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Uploads");
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageModel.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageModel.ImageFile.CopyTo(fileStream);
                }
                //Insert record
                ProductImage productImage = new ProductImage();
                productImage.Id = Guid.NewGuid();
                productImage.Name = uniqueFileName;
                productImage.ProductId = imageModel.ProductId;
                productImage.Extention = extension;
                productImage.RelativePath = filePath;
                _productImageRepository.Create(productImage);
                await _productImageRepository.SaveAsync();
                return RedirectToAction(nameof(Index), new { productId = imageModel.ProductId });
            }
            return View(imageModel);
        }

    }
}
