using Business.Interfaces;
using Business.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationController> _logger;
        public LocationController(ILocationService locationService, ILogger<LocationController> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }

        [HttpGet]
        [Route("getlocations")]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                var locations = await _locationService.GetLocationsFromCsv();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching locations: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("addlocationtocsv")]
        public async Task<IActionResult> AddLocationToCsv([FromBody] AddLocationToCsvRequest request)
        {
            try
            {
                var success = await _locationService.AddLocationToCsv(request);
                if (success)
                    return Ok("Location added successfully");
                else
                    return StatusCode(500, "Failed to add location to CSV");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error adding location to CSV: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
