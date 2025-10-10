using Microsoft.AspNetCore.Mvc;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClient;
        private readonly IConfiguration configuration;
        public RegionsController(IHttpClientFactory _httpClient, IConfiguration _configuration)
        {
            this.httpClient = _httpClient;
            this.configuration = _configuration;
        }
        public async Task<IActionResult>Index()
        {
            //Get region url from appsettings.json
            var regionUrl = configuration["NZWalksAPIs:Region"];

            
            //Call Regions web API
            var responseMessage = await httpClient.CreateClient().GetAsync(requestUri:regionUrl);
            responseMessage.EnsureSuccessStatusCode();
            //Get all regions
            var regions = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<NZWalks.UI.Models.DTO.RegionDto>>();
            //Pass the data to the view
            return View(regions);
        }
    }
}
