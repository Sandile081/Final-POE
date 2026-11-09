using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Models;
using RaceDayManagementSystem.Services;

namespace RaceDayManagementSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventApiServices _eventApi;

        public EventsController(EventApiServices eventApi)
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
                
                var events = await _eventApi.GetAllAsync();
                return View(events);
            }
            catch (HttpRequestException)
            {
                return View("ApiError");
            }
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        // IN YOUR MVC PROJECT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvents(Events events)
        {
            if (!ModelState.IsValid)
                return View(events);

            try
            {
                int? MyOrganiserID = HttpContext.Session.GetInt32("OrganiserID");
                int? MyLocationID = HttpContext.Session.GetInt32("LocationID");
                int? MyCategoryID = HttpContext.Session.GetInt32("CategoriesID");

                if (MyOrganiserID == null || MyLocationID == null || MyCategoryID == null)
                {
                    ModelState.AddModelError("", "Missing Location or Category data.");
                    return View(events);
                }

                events.OrganiserID = MyOrganiserID.Value;
                events.LocationID = MyLocationID.Value;
                events.CategoriesID = MyCategoryID.Value;

              
                var createdEvent = await _eventApi.CreateAsync(events);

                if (createdEvent == null)
                {
                    ModelState.AddModelError("", "The API could not create the Event.");
                    return View(events);
                }

                TempData["Success"] = "Event created successfully.";

              
                return RedirectToAction("UploadBanner", "Events", new { id = createdEvent.EventID });
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("", "Could not connect to the Event API.");
                return View(events);
            }
        }

        public async Task<IActionResult> OrganiserEvents()
        {
            try
            {
                int? organiserId = HttpContext.Session.GetInt32("OrganiserID");

                if (organiserId == null)
                {
                    return RedirectToAction("Index", "Login"); 
                }

                var events = await _eventApi.GetEventForOrganiser(organiserId.Value);

                var FindAllEvents = new List<Events>();

                foreach(var ev in events)
                {
                    var counter = await _eventApi.GetTheNumberOfEnrolment(ev.EventID);

                    int num = counter.Value;

                    FindAllEvents.Add(new Events
                    {
                        EventID = ev.EventID,
                        EventName = ev.EventName,
                        Description = ev.Description,
                        Date = ev.Date,
                        Distance = ev.Distance,
                        EventType = ev.EventType,
                        NumberOfEnrolment = num
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

        public async Task<IActionResult> Details(int id)
        {
            var find = await _eventApi.GetByIdAsync(id);

            if(find == null)
                return NotFound();

            return View(find);
        }

        public async Task<IActionResult> LocationDetails(int id)
        {
            var find = await _eventApi.GetLocationIdAsync(id) ;

            if (find == null)
                return NotFound();

            return View(find);
        }

        public async Task<IActionResult> CategoryDetails(int id)
        {
            var find = await _eventApi.GetCategoryIdAsync(id);

            if (find == null)
                return NotFound();

            return View(find);
        }

        public async Task<IActionResult> UserLocation(int id)
        {
            var find = await _eventApi.GetLocationIdAsync(id);

            if (find == null)
                return NotFound();

            return View(find);
        }

        public async Task<IActionResult> UserCategory(int id)
        {
            var find = await _eventApi.GetCategoryIdAsync(id);

            if (find == null)
                return NotFound();

            return View(find);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var events = await _eventApi.GetByIdAsync(id);
            if (events == null)
                return NotFound();

            return View(events);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEvent(int id, Events events)
        {
            if (id != events.EventID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("Edit", events);

            try
            {
                if (!await _eventApi.UpdateAsync(id, events))
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not update the Event.");
                    return View("Edit", events);
                }

                TempData["Success"] = "Event updated successfully.";
                return RedirectToAction("OrganiserEvents", "Events");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Event API.");
                return View("Edit", events);
            }
        }


        public async Task<IActionResult> EditLocation(int id)
        {
            var events = await _eventApi.GetLocationIdAsync(id);
            if (events == null)
                return NotFound();

            return View(events);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLocation(int id, Location location)
        {
            if (id != location.LocationID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("EditLocation", location);

            try
            {
                if (!await _eventApi.UpdateLocationAsync(id, location))
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not update the Location.");
                    return View("EditLocation", location);
                }

                TempData["Success"] = "Location updated successfully.";
                return RedirectToAction("OrganiserEvents", "Events");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Location API.");
                return View("EditLocation", location);
            }
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var events = await _eventApi.GetCategoryIdAsync(id);
            if (events == null)
                return NotFound();

            return View(events);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(int id, Categories categories)
        {
            if (id != categories.CategoriesID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("UpdateCategory", categories);

            try
            {
                if (!await _eventApi.UpdateCategoryAsync(id, categories))
                {
                    ModelState.AddModelError(
                    string.Empty,
                    "The API could not update the Category.");
                    return View("UpdateCategory", categories);
                }

                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction("OrganiserEvents", "Events");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                string.Empty,
                "Could not connect to the Category API.");
                return View("UpdateCategory", categories);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var Event = await _eventApi.GetByIdAsync(id);
            if (Event == null)
                return NotFound();

            return View(Event);
        }

        // DELETE: Process confirmed deletion.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id, Events events)
        {
            try
            {
                var Event = await _eventApi.GetByIdAsync(id);

                if (!await _eventApi.DeleteAsync(id))
                {
                    TempData["Error"] =
                    "The API could not delete the Event.";
                    return RedirectToAction("OrganiserEvents", "Events");
                }

                if(!await _eventApi.DeleteLocationAsync(Event.LocationID))
                {
                    TempData["Error"] =
                   "The API could not delete the Location.";
                    return RedirectToAction("OrganiserEvents", "Events");
                }

                if (!await _eventApi.DeleteCategoryAsync(Event.CategoriesID))
                {
                    TempData["Error"] =
                   "The API could not delete the Category.";
                    return RedirectToAction("OrganiserEvents", "Events");
                }


                TempData["Success"] = "Event deleted successfully.";
                return RedirectToAction("OrganiserEvents", "Events");
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                "Could not connect to the Event API.";
                return RedirectToAction("OrganiserEvents", "Events");
            }
        }

        // ADD THIS TO YOUR EventsController
        [HttpGet]
        public IActionResult UploadBanner(int id)
        {
            // We pass the ID to the View so the form knows which event this image belongs to
            ViewBag.EventID = id;
            return View();
        }

        // (Your [HttpPost] UploadBanner method goes here)


        [HttpPost]
        public async Task<IActionResult> UploadBanner(int eventId, IFormFile bannerFile)
        {
            if (bannerFile != null && bannerFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(bannerFile.FileName);
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                var filePath = Path.Combine(uploadFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await bannerFile.CopyToAsync(stream);
                }

                var imageUrl = $"/images/banners/{fileName}";

                // 🚨 LOOK AT THIS LINE. THIS IS WHERE IT MIGHT BE FAILING.
                bool success = await _eventApi.UpdateBannerUrlAsync(eventId, imageUrl);

                if (success)
                {
                    TempData["Success"] = "Banner uploaded successfully!";
                    return RedirectToAction("OrganiserEvents");
                }
                else
                {
                    ModelState.AddModelError("", "The API failed to save the image URL.");
                }
            }

            ModelState.AddModelError("", "Please select a file.");
            ViewBag.EventID = eventId;
            return View();
        }

     

        [HttpGet]
        public async Task<IActionResult> Enrol(int eventId)
        {

            int? participantId = HttpContext.Session.GetInt32("ParticipantID");

            if (participantId == null)
                return RedirectToAction("Index", "Login");


            var eventDetails = await _eventApi.GetByIdAsync(eventId);
            if (eventDetails == null)
                return NotFound();

            
            ViewBag.EventID = eventId;
            ViewBag.EventName = eventDetails.EventName;
            ViewBag.CategoriesID = eventDetails.CategoriesID;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(int eventId, int age)
        {
            int? participantId = HttpContext.Session.GetInt32("ParticipantID");

            if (participantId == null)
                return RedirectToAction("Index", "Login");

            try
            {
                // Just save it!
                EventEnrolment eventEnrolment = new EventEnrolment
                {
                    ParticipantID = participantId.Value,
                    EnrolmentStatus = "Pending",
                    EventID = eventId
                };

                Result resulting = new Result
                {
                    FinishingPosition = "Pending",
                    Time="0h00",
                    ParticipantID = participantId.Value,
                    OrganiserID=0
               
                };

                var find = await _eventApi.CreateEventEnrolmentAsync(eventEnrolment);

                var CreateResults = await _eventApi.CreateResuiltsAsync(resulting);


                if (find == null)
                {
                    ModelState.AddModelError("", "The API returned null. Check the API project.");
                    ViewBag.EventID = eventId;
                    return View();
                }

                TempData["Success"] = "Participant Enrolment successful.";

                return RedirectToAction("Index", "Events");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("", "Could not connect to the Event API.");
                ViewBag.EventID = eventId;
                return View();
            }
        }




    }
}
