﻿using Ecommerce.Admin.ViewModels;
using Ecommerce.Infrastructure.Domains;
using Ecommerce.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Admin.Controllers
{
    public class ProductsController : Controller
    {
        public readonly IProductRepository _productRepository;
        public readonly IProductCategoryRepository _productCategoryRepository;
        public ProductsController(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }
        public async Task<ActionResult<IList<ProductViewModel>>> Index()
        {
            var data = await _productRepository.FindAll().Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
            }).OrderBy(ow => ow.Name).ToListAsync();

            return View(data);
        }
      
        public async Task<ActionResult> Create()
        {
            var data = await _productCategoryRepository.FindAll().Select(x => new DropDownList { Value = x.Id, Text = x.Name }).ToListAsync();
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.DropDownList= data;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductViewModel viewModel)
        {
            try
            {
                Product product = new Product();
                product.Id = Guid.NewGuid();
                product.Name = viewModel.Name;
                product.SalePrice = viewModel.SalePrice;
                product.Description = viewModel.Description;
                product.Title = viewModel.Title;
                product.ModelNumber = viewModel.ModelNumber;
                product.Size = viewModel.Size;
                product.MRP = viewModel.MRP;
                product.Colour = viewModel.Colour;
                product.Brand = viewModel.Brand;
                product.ProductCategoryId = viewModel.ProductCategoryId;
                _productRepository.Create(product);
                await _productRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var product = await _productRepository.FindByCondition(x => x.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return View();
            }
            var data = await _productCategoryRepository.FindAll().Select(x => new DropDownList { Value = x.Id, Text = x.Name }).ToListAsync();
            ProductViewModel viewModel = new ProductViewModel();          
            viewModel.Id = id;
            viewModel.Name = product.Name;
            viewModel.SalePrice = product.SalePrice;
            viewModel.ModelNumber = product.ModelNumber;
            viewModel.Title = product.Title;
            viewModel.Description = product.Description;
            viewModel.Size = product.Size;
            viewModel.MRP = product.MRP;
            viewModel.Colour = product.Colour;
            viewModel.Brand = product.Brand;
            viewModel.ProductCategoryId = product.ProductCategoryId;
            viewModel.DropDownList = data;
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, ProductViewModel viewModel)
        {
            try
            {
                if (id != viewModel.Id)
                {
                    return View();
                }
                var product = await _productRepository.FindByCondition(x => x.Id == viewModel.Id).FirstOrDefaultAsync();
                if (product == null)
                {
                    return View();
                }
                product.Name = viewModel.Name;
                product.SalePrice = viewModel.SalePrice;
                product.ModelNumber = viewModel.ModelNumber;
                product.Description = viewModel.Description;
                product.Title = viewModel.Title;
                product.Size = viewModel.Size;
                product.MRP = viewModel.MRP;
                product.Colour = viewModel.Colour;
                product.Brand = viewModel.Brand;
                product.ProductCategoryId = viewModel.ProductCategoryId;
                _productRepository.Update(product);
                await _productRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            var product = await _productRepository.FindByCondition(x => x.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return View();
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Id = id;
            viewModel.Name = product.Name;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ProductViewModel viewModel)
        {
            try
            {
                var product = await _productRepository.FindByCondition(x => x.Id == viewModel.Id).FirstOrDefaultAsync();
                if (product == null)
                {
                    return View();
                }
                _productRepository.Delete(product);
                await _productRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
