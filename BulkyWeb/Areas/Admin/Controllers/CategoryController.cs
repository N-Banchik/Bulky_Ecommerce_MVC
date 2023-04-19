using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> categories = new List<Category>();
            categories = (List<Category>)_unitOfWork.Category.GetAll();
            return View(categories);
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot match the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Category created successful";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult EditCategory(int? id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            Category? CategoryToEdit = _unitOfWork.Category.Get(c => c.Id == id);
            if (CategoryToEdit == null)
            {
                return NotFound();
            }
            return View(CategoryToEdit);
        }
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot match the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.SaveChanges();
                TempData["success"] = "Category updated successful";
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult DeleteCategory(int? id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            Category? CategoryToDelete = _unitOfWork.Category.Get(c => c.Id == id);
            if (CategoryToDelete == null)
            {
                return NotFound();
            }
            return View(CategoryToDelete);
        }
        [HttpPost]
        public IActionResult DeleteCategory(Category category)
        {

            _unitOfWork.Category.Remove(category);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Category Deleted successful";
            return RedirectToAction("Index");



        }
    }
}
