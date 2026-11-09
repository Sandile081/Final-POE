using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Models;
using RaceDayManagementSystem.Services;

namespace RaceDayManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly EventApiServices _eventApi;

        public LoginController(EventApiServices eventApi)
        {
            _eventApi = eventApi;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ParticipantLogin()
        {
            return View();
        }
        public IActionResult OrganiserLogin()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoggingIn(OrganiserLogin organiserLogin)
        {
          
            var isLoginSuccessful = await _eventApi.VerifyingOrganiser(organiserLogin);

            

            if (isLoginSuccessful == null) 
            {
                ModelState.AddModelError("", "Invalid login credentials.");
                return RedirectToAction(nameof(Index));
            }

            HttpContext.Session.SetInt32("OrganiserID", isLoginSuccessful.Value);

           

            return RedirectToAction("OrganiserEvents", "Events");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoggingInToo(ParticipantLogin participantLogin)
        {

            var isLoginSuccessful = await _eventApi.VerifyingParticipant(participantLogin);

            if (isLoginSuccessful == null)
            {
                ModelState.AddModelError("", "Invalid login credentials.");
                return RedirectToAction(nameof(Index));
            }

            HttpContext.Session.SetInt32("ParticipantID", isLoginSuccessful.Value);


            return RedirectToAction("Index", "Events");

        }
    }
}
