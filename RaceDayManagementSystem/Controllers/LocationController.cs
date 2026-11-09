using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Models;
using RaceDayManagementSystem.Services;

namespace RaceDayManagementSystem.Controllers
{
    public class LocationController : Controller
    {
        private readonly EventApiServices _eventApi;

        public LocationController(EventApiServices eventApi)
        {
            _eventApi = eventApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLocation(Location location)
        {
            if (!ModelState.IsValid)
                return View(location);

            try
            {

                var CreatingLocation = await _eventApi.CreateLocation(location);

                if (CreatingLocation == null)
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not create the Location.");
                    return View(location);
                }

               


                HttpContext.Session.SetInt32("LocationID", CreatingLocation.LocationID);
               

                TempData["Success"] = "Location created successfully.";
                return RedirectToAction("Index", "Categories");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Location API.");
                return View(location);
            }
        }
    }
}
