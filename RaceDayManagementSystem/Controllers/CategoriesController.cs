using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Models;
using RaceDayManagementSystem.Services;

namespace RaceDayManagementSystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly EventApiServices _eventApi;

        public CategoriesController(EventApiServices eventApi)
        {
            _eventApi = eventApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Categories categories)
        {
            if (!ModelState.IsValid)
                return View(categories);

            try
            {
                var CreatingCategory = await _eventApi.CreateCategory(categories);

                if (CreatingCategory == null)
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not create the Category.");
                    return View(categories);
                }

                

                HttpContext.Session.SetInt32("CategoriesID", CreatingCategory.CategoriesID);

                TempData["Success"] = "Category created successfully.";
                return RedirectToAction("Create","Events");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Location API.");
                return View(categories);
            }
        }
    }
}
