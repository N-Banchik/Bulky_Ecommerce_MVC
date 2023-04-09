using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {

            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            List<Category> categories = new List<Category>();
            categories = (List<Category>)_categoryRepository.GetAll();
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
                _categoryRepository.Add(category);
                _categoryRepository.SaveChanges();
                TempData["success"] = "Category created successful";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult EditCategory(int? id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            Category? CategoryToEdit = _categoryRepository.Get(c => c.Id == id);
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
                _categoryRepository.Update(category);
                _categoryRepository.SaveChanges();
                TempData["success"] = "Category updated successful";
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult DeleteCategory(int? id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            Category? CategoryToDelete = _categoryRepository.Get(c => c.Id == id);
            if (CategoryToDelete == null)
            {
                return NotFound();
            }
            return View(CategoryToDelete);
        }
        [HttpPost]
        public IActionResult DeleteCategory(Category category)
        {

            _categoryRepository.Remove(category);
            _categoryRepository.SaveChanges();
            TempData["success"] = "Category Deleted successful";
            return RedirectToAction("Index");



        }
    }
}
