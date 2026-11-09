using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Models;
using RaceDayManagementSystem.Services;

namespace RaceDayManagementSystem.Controllers
{
    public class OrganiserController : Controller
    {

        private readonly EventApiServices _eventApi;

        public OrganiserController(EventApiServices eventApi)
        {
            _eventApi = eventApi;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(Organiser organiser)
        {
            HttpContext.Session.SetInt32("OrganiserID", organiser.OrganiserID);


            int? organiserId = HttpContext.Session.GetInt32("OrganiserID");

            if (!ModelState.IsValid)
                return View(organiser);
         

            try
            {


                if (!await _eventApi.OrganiserCreated(organiser))
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not create the Organiser.");
                    return RedirectToAction("index", "Login");
                }

                if (organiserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                var events = await _eventApi.GetEventForOrganiser(organiserId.Value);

                TempData["Success"] = "Organiser created successfully.";
                return RedirectToAction("OrganiserEvents", "Events");


            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Event API.");
                return View(organiser);
            }
        }
    }
}
