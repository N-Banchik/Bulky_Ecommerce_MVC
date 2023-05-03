using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;
        private IEnumerable<SelectListItem> _categoryList;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHost;
            _categoryList = new List<SelectListItem>();
        }
        public IActionResult Index()
        {
            List<Product> products = (List<Product>)_unitOfWork.Product.GetAll(includeProps: "Category");

            return View(products);
        }
        public IActionResult UpsertProduct(int? id)
        {
            _categoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            ProductVM productVM = new ProductVM()
            {
                CategoryList = _categoryList
            };
            if (id == null || id == 0)
            {
                productVM.Product = new();
                return View(productVM);

            }
            else
            {
                Product? ProductToEdit = _unitOfWork.Product.Get(c => c.Id == id);
                if (ProductToEdit == null)
                {
                    return NotFound();
                }
                productVM.Product = ProductToEdit;
                return View(productVM);
            }

        }
        [HttpPost]
        public IActionResult UpsertProduct(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _webHost.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                    }
                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;

                }
                else if (string.IsNullOrEmpty(productVM.Product.ImageUrl))
                {
                    productVM.Product.ImageUrl = "";
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);

                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.SaveChanges();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult DeleteProduct(int? id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            Product? ProductToDelete = _unitOfWork.Product.Get(c => c.Id == id);
            if (ProductToDelete == null)
            {
                return NotFound();
            }
            return View(ProductToDelete);
        }
        [HttpPost]
        public IActionResult DeleteProduct(Product product)
        {

            _unitOfWork.Product.Remove(product);
            _unitOfWork.SaveChanges();
            if (product.ImageUrl != null)
            {
                string oldImagePath = Path.Combine(_webHost.WebRootPath, product.ImageUrl.Trim('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            TempData["success"] = "Product Deleted successful";
            return RedirectToAction("Index");
        }
        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = (List<Product>)_unitOfWork.Product.GetAll(includeProps: "Category");
            return Json(new { data = products });
        }
        #endregion
    }
}
