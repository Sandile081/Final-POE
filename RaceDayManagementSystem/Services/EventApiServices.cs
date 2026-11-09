using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RaceDayManagementSystem.Models;
using System.Net;
using System.Net.Http.Json;


namespace RaceDayManagementSystem.Services
{
    public class EventApiServices
    {
        private readonly HttpClient _httpClient;

        public EventApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //==================================================================================================
        //============================================EVENT API=============================================
        //==================================================================================================

        public async Task<List<Events>> GetAllAsync()
        {
            var FindEvents = await _httpClient.GetFromJsonAsync<List<Events>>("api/Events");

            return FindEvents?? new List<Events>();

        }

        public async Task<Events> GetByIdAsync(int id)
        {
            var FindEvent = await _httpClient.GetAsync($"api/Events/{id}");

            if (FindEvent.StatusCode == HttpStatusCode.NotFound)
                return null;

            FindEvent.EnsureSuccessStatusCode();

            return await FindEvent.Content.ReadFromJsonAsync<Events>();

        }

        public async Task<Events?> CreateAsync(Events events)
        {
            var FindEvent = await _httpClient.PostAsJsonAsync(
            "api/Events", events);

            if (FindEvent.IsSuccessStatusCode)
            {
                // ✅ Read the event (with the new ID) back from the API
                return await FindEvent.Content.ReadFromJsonAsync<Events>();
            }
            return null;
        }

        public async Task<bool> UpdateAsync(int id, Events events)
        {
            var FindEvent = await _httpClient.PutAsJsonAsync(
            $"api/Events/{id}", events);

            return FindEvent.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var FindEvents = await _httpClient.DeleteAsync(
            $"api/Events/{id}");

            return FindEvents.IsSuccessStatusCode;
        }

        //==================================================================================================
        //============================================Login Verification====================================
        //==================================================================================================

        public async Task<int?> VerifyingOrganiser(OrganiserLogin organiserLogin)
        {
            var FindOrganiser = await _httpClient.PostAsJsonAsync($"api/OrganiserLogin",organiserLogin);

            if(FindOrganiser.IsSuccessStatusCode)
            {
                return await FindOrganiser.Content.ReadFromJsonAsync<int>();
            }

            return null;
        }

        public async Task<int?> VerifyingParticipant(ParticipantLogin participantLogin)
        {
            var FindOrganiser = await _httpClient.PostAsJsonAsync($"api/ParticipantLogin", participantLogin);

            if (FindOrganiser.IsSuccessStatusCode)
            {
                return await FindOrganiser.Content.ReadFromJsonAsync<int>();
            }

            return null;
        }
        //==================================================================================================
        //============================================REGISTRACTION=========================================
        //==================================================================================================

        public async Task<bool> OrganiserCreated(Organiser organiser)
        {
            var AddingOrganiser = await _httpClient.PostAsJsonAsync($"api/Organiser",organiser);

            return AddingOrganiser.IsSuccessStatusCode;
        }

        public async Task<Participant?> ParticipantCreated(Participant participant)
        {
            var AddingParticipant = await _httpClient.PostAsJsonAsync($"api/Participant", participant);

            if (AddingParticipant.IsSuccessStatusCode)
            {
                // Return the created participant with the new ID!
                return await AddingParticipant.Content.ReadFromJsonAsync<Participant>();
            }
            return null;
        }
        //==================================================================================================
        //============================================ORGANISERS============================================
        //==================================================================================================

        public async Task<List<Events>> GetEventForOrganiser(int id)
        {
            var FindEvents = await _httpClient.GetFromJsonAsync<List<Events>>($"api/Organiser/{id}/Events");

            return FindEvents ?? new List<Events>();
        }
        //==================================================================================================
        //============================================PARTICIPANTS==========================================
        //==================================================================================================

        public async Task<Participant> GetParticipantByIdAsync(int id)
        {
            var FindEvent = await _httpClient.GetAsync($"api/Participant/{id}");

            if (FindEvent.StatusCode == HttpStatusCode.NotFound)
                return null;

            FindEvent.EnsureSuccessStatusCode();

            return await FindEvent.Content.ReadFromJsonAsync<Participant>();

        }

        public async Task<List<Participant>> GetPEnrolByIdAsync(int id)
        {
            var FindEvent = await _httpClient.GetFromJsonAsync<List<Participant>>($"api/Events/{id}/Participant");

            return FindEvent ?? new List<Participant>();

        }

        public async Task<string?> getParticipantEnrolledAsync(int id)
        {
            var getStatus = await _httpClient.GetAsync($"api/Participant/{id}/EventEnrolment");

            return await getStatus.Content.ReadAsStringAsync();
        }


        public async Task<List<EventEnrolment>> GetAllParticipantEnrolledAsync(int id)
        {
            var FindEvents = await _httpClient.GetFromJsonAsync<List<EventEnrolment>>($"api/Participant/{id}/ParticipantEnrolledEvents");

            return FindEvents ?? new List<EventEnrolment>();

        }

        public async Task<bool> UpdateProfileAsync(int id, Participant participant)
        {
            var FindEvent = await _httpClient.PutAsJsonAsync(
            $"api/Participant/{id}", participant);

            return FindEvent.IsSuccessStatusCode;
        }


        //==================================================================================================
        //============================================EVENTENROLMENT========================================
        //==================================================================================================

        public async Task<int?> GetTheNumberOfEnrolment(int id)
        {
            var findIt = await _httpClient.GetAsync($"api/Events/{id}/EventEnrolment");

            if (findIt.StatusCode == HttpStatusCode.NotFound)
                return null;

           

            findIt.EnsureSuccessStatusCode();

            int num = await findIt.Content.ReadFromJsonAsync<int>();

            return num; 

        }

        public async Task<bool> UpdatesStatus(int id, EventEnrolment eventEnrolment)
        {
            var FindEvent = await _httpClient.PutAsJsonAsync(
            $"api/EventEnrolment/{id}", eventEnrolment);

            return FindEvent.IsSuccessStatusCode;
        }

        public async Task<EventEnrolment> GetSingleEnrolmentAsync(int id)
        {
            var FindEvent = await _httpClient.GetAsync($"api/EventEnrolment/{id}/ParticipantEnrolled");

            if (FindEvent.StatusCode == HttpStatusCode.NotFound)
                return null;

            FindEvent.EnsureSuccessStatusCode();

            return await FindEvent.Content.ReadFromJsonAsync<EventEnrolment>();

        }


        public async Task<EventEnrolment?> CreateEventEnrolmentAsync(EventEnrolment eventEnrolment)
        {
            var FindEvent = await _httpClient.PostAsJsonAsync("api/EventEnrolment", eventEnrolment);

            if (FindEvent.IsSuccessStatusCode)
            {
                
                return await FindEvent.Content.ReadFromJsonAsync<EventEnrolment>();
            }
            return null;
        }

        public async Task<bool> UpdateResultsAsync(int id, Result result)
        {
            var FindEvent = await _httpClient.PutAsJsonAsync(
            $"api/Results/{id}", result);

            return FindEvent.IsSuccessStatusCode;
        }

        //==================================================================================================
        //============================================LOCATION==============================================
        //==================================================================================================

        public async Task<Location?> CreateLocation(Location location)
        {
            var AddLocation = await _httpClient.PostAsJsonAsync($"/api/Location", location);

            if (AddLocation.IsSuccessStatusCode)
            {
                return await AddLocation.Content.ReadFromJsonAsync<Location>();
            }

            return null;
        }

        public async Task<Location> GetLocationIdAsync(int id)
        {
            var FindEvent = await _httpClient.GetAsync($"api/Location/{id}");

            if (FindEvent.StatusCode == HttpStatusCode.NotFound)
                return null;

            FindEvent.EnsureSuccessStatusCode();

            return await FindEvent.Content.ReadFromJsonAsync<Location>();

        }

        public async Task<bool> UpdateLocationAsync(int id, Location location)
        {
            var FindEvent = await _httpClient.PutAsJsonAsync(
            $"api/Location/{id}", location);

            return FindEvent.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var FindEvents = await _httpClient.DeleteAsync(
            $"api/Location/{id}");

            return FindEvents.IsSuccessStatusCode;
        }

        //==================================================================================================
        //============================================CATEGORY==============================================
        //==================================================================================================

        public async Task<Categories?> CreateCategory(Categories categories)
        {
            var AddCategory = await _httpClient.PostAsJsonAsync($"api/Categories",categories);

            if(AddCategory.IsSuccessStatusCode)
            {
                return await AddCategory.Content.ReadFromJsonAsync<Categories>();
            }
            

            return null;
        }

        public async Task<Categories> GetCategoryIdAsync(int id)
        {
            var FindEvent = await _httpClient.GetAsync($"api/Categories/{id}");

            if (FindEvent.StatusCode == HttpStatusCode.NotFound)
                return null;

            FindEvent.EnsureSuccessStatusCode();

            return await FindEvent.Content.ReadFromJsonAsync<Categories>();

        }

        public async Task<bool> UpdateCategoryAsync(int id, Categories categories)
        {
            var FindEvent = await _httpClient.PutAsJsonAsync(
            $"api/Categories/{id}", categories);

            return FindEvent.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var FindEvents = await _httpClient.DeleteAsync(
            $"api/Categories/{id}");

            return FindEvents.IsSuccessStatusCode;
        }

        public async Task<int?> GetCategoryAge(int id)
        {
            var findAge = await _httpClient.GetAsync($"Categories/{id}/ParticipantAge");

            if (findAge.StatusCode == HttpStatusCode.NotFound)
                return null;



            findAge.EnsureSuccessStatusCode();

            int num = await findAge.Content.ReadFromJsonAsync<int>();

            return num;

        }

        //==================================================================================================
        //============================================BANNER IMAGES=========================================
        //==================================================================================================

        // In your MVC Project
        public async Task<bool> UpdateBannerUrlAsync(int eventId, string bannerUrl)
        {
            var payload = new { BannerUrl = bannerUrl };
            var response = await _httpClient.PutAsJsonAsync($"api/Events/{eventId}/banner", payload);

            // Add this so you can see if the API is rejecting it!
            var errorText = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }

        //==================================================================================================
        //============================================RESULT================================================
        //==================================================================================================
       
        public async Task<Result?> GetPartcipantResultsByIdAsync(int id)
        {
            var FindEvent = await _httpClient.GetAsync($"api/Participant/{id}/Result");

            if (FindEvent.StatusCode == HttpStatusCode.NotFound)
                return null;

            FindEvent.EnsureSuccessStatusCode();

            return await FindEvent.Content.ReadFromJsonAsync<Result>();
        }

        public async Task<List<Result>> GetAllResultsAsync(int id)
        {
            var FindEvents = await _httpClient.GetFromJsonAsync<List<Result>>($"api/Participant/{id}/History");

            return FindEvents ?? new List<Result>();

        }

        public async Task<Result> CreateResuiltsAsync(Result result)
        {
            var FindEvent = await _httpClient.PostAsJsonAsync(
            "api/Results", result);

            if (FindEvent.IsSuccessStatusCode)
            {
                
                return await FindEvent.Content.ReadFromJsonAsync<Result>();
            }
            return null;
        }




    }
}
