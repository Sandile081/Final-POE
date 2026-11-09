using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Services;

namespace RaceDayManagementSystem.Controllers
{
    public class ResultController : Controller
    {
        private readonly EventApiServices _eventApi;

        public ResultController(EventApiServices eventApi)
        {
            _eventApi = eventApi;
        }

      
        public async Task<IActionResult> Index()
        {
            int? participantId = HttpContext.Session.GetInt32("ParticipantID");

            if (participantId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var results = await _eventApi.GetPartcipantResultsByIdAsync(participantId.Value);
                return View(results); 
            }
            catch (HttpRequestException)
            {
                return View("ApiError");
            }
        }

        public async Task<IActionResult> History()
        {
            int? participantId = HttpContext.Session.GetInt32("ParticipantID");

            if (participantId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {

                var events = await _eventApi.GetAllResultsAsync(participantId.Value);

                return View(events);
            }
            catch (HttpRequestException)
            {
                return View("ApiError");
            }
        }
    }
}
