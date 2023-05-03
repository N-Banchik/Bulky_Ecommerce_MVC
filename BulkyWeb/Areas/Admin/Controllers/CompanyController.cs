using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }
        public IActionResult UpsertCompany(int? id)
        {

            Company company = new Company();

            if (id == null || id == 0)
            {
                company = new();
                return View(company);

            }
            else
            {
                Company? companyToEdit = _unitOfWork.Company.Get(c => c.Id == id);
                if (companyToEdit == null)
                {
                    return NotFound();
                }

                return View(companyToEdit);
            }

        }
        [HttpPost]
        public IActionResult UpsertCompany(Company company)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Update(company);
                _unitOfWork.SaveChanges();
                TempData["success"] = "New Company created successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult DeleteCompany(int? id)
        {
            if (id == null || id == 0)
            { return NotFound(); }
            Company?companyToDelete = _unitOfWork.Company.Get(c => c.Id == id);
            if (companyToDelete == null)
            {
                return NotFound();
            }
            return View(companyToDelete);
        }
        [HttpPost]
        public IActionResult DeleteCompany(Company company)
        {

            _unitOfWork.Company.Remove(company);
            _unitOfWork.SaveChanges();
           
            TempData["success"] = "Company Deleted successful";
            return RedirectToAction("Index");
        }


        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companies });
        }
        #endregion
    }
}
