using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Models;
using RaceDayManagementSystem.Services;

namespace RaceDayManagementSystem.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly EventApiServices _eventApi;

        public ParticipantController(EventApiServices eventApi)
        {
            _eventApi = eventApi;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(Participant participant)
        {
            if (!ModelState.IsValid)
                return View(participant);

            try
            {
                
                var createdParticipant = await _eventApi.ParticipantCreated(participant);

                if (createdParticipant == null)
                {
                    ModelState.AddModelError(string.Empty, "The API could not create the participant.");
                    return View(participant);
                }

             
                HttpContext.Session.SetInt32("ParticipantID", createdParticipant.ParticipantID);

                TempData["Success"] = "Registration successful! Welcome.";

                
                return RedirectToAction("Index", "Events");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Could not connect to the Event API.");
                return View(participant);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditParticipant()
        {
            int? participantId = HttpContext.Session.GetInt32("ParticipantID");

            if (participantId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var participant = await _eventApi.GetParticipantByIdAsync(participantId.Value);

            if (participant == null)
                return NotFound();

            return View(participant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, Participant participant)
        {
            if (id != participant.ParticipantID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(participant);

            try
            {
                if (!await _eventApi.UpdateProfileAsync(id, participant))
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not update the student.");
                    return View(participant);
                }

                TempData["Success"] = "Updated Profile successfully.";
                return RedirectToAction("Index","Events");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Participant API.");
                return View(participant);
            }
        }



    }
}
