using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Models;
using RaceDayManagementSystem.Services;

namespace RaceDayManagementSystem.Controllers
{
    public class EventEnrolmentController : Controller
    {
        private readonly EventApiServices _eventApi;

        public EventEnrolmentController(EventApiServices eventApi)
        {
            _eventApi = eventApi;
        }

        public async Task<IActionResult> Index(int id)
        {
            try
            {


                var events = await _eventApi.GetByIdAsync(id);

                var EnrolledParticipants = await _eventApi.GetPEnrolByIdAsync(id);

                

                var FindAllEvents = new List<Participant>();

               

                foreach (var ep in EnrolledParticipants)
                {

                    var status = await _eventApi.getParticipantEnrolledAsync(ep.ParticipantID);

                    if (status == null)
                        return NotFound();


                    FindAllEvents.Add(new Participant
                    {
                        ParticipantID = ep.ParticipantID,
                        Pname = ep.Pname,
                        Psurname = ep.Psurname,
                        SelectedCatagory = events.EventType.ToString(),
                        EnrolmentStatus = status.ToString()


                    }

                 );
                }

                return View(FindAllEvents);
            }
            catch (HttpRequestException)
            {
                return View("ApiError");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id)
        {

            var events = await _eventApi.GetSingleEnrolmentAsync(id);
            if (events == null)
                return NotFound();

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, EventEnrolment eventEnrolment)
        {
            if (id != eventEnrolment.EventEnrolmentID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("UpdateStatus", eventEnrolment);

            try
            {
                if (!await _eventApi.UpdatesStatus(id,eventEnrolment))
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not update the participant Status.");
                    return View("Edit", eventEnrolment);
                }

                TempData["Success"] = "Participant Status updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Enrollment API.");
                return View("UpdateStatus", eventEnrolment);
            }
        }

        public async Task<IActionResult> ParticipantEnrolled()
        {
            int? participantId = HttpContext.Session.GetInt32("ParticipantID");

            if (participantId == null)
            {
                return RedirectToAction("Index", "Login");
            }


            try
            {

                var events = await _eventApi.GetAllParticipantEnrolledAsync(participantId.Value);
                return View(events);
            }
            catch (HttpRequestException)
            {
                return View("ApiError");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditResults(int id)
        {
            var result = await _eventApi.GetPartcipantResultsByIdAsync(id);
            if (result == null)
                return NotFound();

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResults(int id, Result result)
        {
            if (id != result.ResultID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(result);

            try
            {
                if (!await _eventApi.UpdateResultsAsync(id, result))
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not update the results.");
                    return View(result);
                }

                TempData["Success"] = "Updated Results successful.";
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Results API.");
                return View(result);
            }
        }





    }
}
