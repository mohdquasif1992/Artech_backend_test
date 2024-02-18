using Business.Interfaces;
using Business.Requests;
using Business.Responses;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System.Globalization;


namespace Business.Services
{
    public class LocationService : ILocationService
    {

        private readonly ILogger<LocationService> _logger;
        private readonly string _csvFilePath = "../Data/Data/locations.csv";
        
        public LocationService(ILogger<LocationService> logger)
        {
            _logger = logger;
          
        }
        public async Task<List<LocationResponse>> GetLocationsFromCsv()
        {
            try
            {
                using (var reader = new StreamReader(_csvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = await csv.GetRecordsAsync<LocationResponse>().ToListAsync();

                    _logger.LogInformation($"Total records read from CSV: {records.Count}");

                  
                    var availableLocations = records.Where(location =>
                    {
                        if (!string.IsNullOrEmpty(location.StartTime) && !string.IsNullOrEmpty(location.EndTime))
                        {
                          
                            if (TimeSpan.TryParseExact(location.StartTime, "h\\:mm", CultureInfo.InvariantCulture, out var startTime) &&
                                TimeSpan.TryParseExact(location.EndTime, "h\\:mm", CultureInfo.InvariantCulture, out var endTime))
                            {
                               
                                bool isAvailable = startTime >= TimeSpan.FromHours(10) && endTime <= TimeSpan.FromHours(13);
                                if (!isAvailable)
                                {
                                    _logger.LogInformation($"Location {location.LocationId} is not available between 10 am and 1 pm.");
                                }
                                return isAvailable;
                            }
                        }
                        _logger.LogInformation($"Invalid StartTime or EndTime for Location {location.LocationId}.");
                        return false;
                    }).ToList();

                    _logger.LogInformation($"Total available locations: {availableLocations.Count}");

                    return availableLocations;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error reading locations from CSV: {ex.Message}");
                throw;
            }
        }
        public Task<bool> AddLocationToCsv(AddLocationToCsvRequest location)
        {
            try
            {

                using (var writer = new StreamWriter(_csvFilePath, append: true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    writer.WriteLine();
                    csv.WriteRecord(location);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding location to CSV: {ex.Message}");
                return Task.FromResult(false);
            }
        }


    }
}
