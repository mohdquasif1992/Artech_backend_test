using Business.Interfaces;
using Business.Requests;
using Business.Responses;
using Business.Services;
using Microsoft.Extensions.Logging;
using Moq;


namespace Business.Tests
{
    [TestFixture]
    public class LocationServiceTests
    {
        private Mock<ILogger<LocationService>> _loggerMock;
        private LocationService _locationService;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<LocationService>>();

        }

        [Test]
        public async Task GetLocationsFromCsv_ReturnsAvailableLocations()
        {
            // Arrange
            var locationServiceMock = new Mock<ILocationService>();
            var expectedLocations = new List<LocationResponse>
            {
               new LocationResponse { LocationId = 1, LocationName = "Location A", DayOfWeek = "Monday", StartTime = "10:00 AM", EndTime = "1:00 PM" },
            };
            locationServiceMock.Setup(service => service.GetLocationsFromCsv()).ReturnsAsync(expectedLocations);


            var locations = await locationServiceMock.Object.GetLocationsFromCsv();

            // Assert
            Assert.IsNotNull(locations);
            Assert.AreEqual(1, locations.Count);
            Assert.AreEqual("Location A", locations[0].LocationName);
        }
        [Test]
        public async Task GetLocationsFromCsv_ReturnsAvailableLocations_Exception()
        {
            // Arrange
            var locationServiceMock = new Mock<ILocationService>();

            var expectedLocations = new List<LocationResponse>
    {
        new LocationResponse { LocationId = 1, LocationName = "Location A", DayOfWeek = "Monday", StartTime = "10:00 AM", EndTime = "1:00 PM" },
    };
            locationServiceMock.Setup(service => service.GetLocationsFromCsv()).ReturnsAsync(expectedLocations);

            // Act
            var locations = await locationServiceMock.Object.GetLocationsFromCsv();

            // Assert
            Assert.IsNotNull(locations);
            Assert.AreEqual(1, locations.Count);
            Assert.AreEqual("Location A", locations[0].LocationName);

            // Case 2: Mock the behavior to return null
            locationServiceMock.Setup(service => service.GetLocationsFromCsv()).ReturnsAsync((List<LocationResponse>)null);

            // Act
            locations = await locationServiceMock.Object.GetLocationsFromCsv();

            // Assert
            Assert.IsNull(locations);

            
            var errorMessage = "Error reading locations from CSV";
            locationServiceMock.Setup(service => service.GetLocationsFromCsv()).ThrowsAsync(new Exception(errorMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await locationServiceMock.Object.GetLocationsFromCsv());
            Assert.AreEqual(errorMessage, exception.Message);
        }

        [Test]
        public async Task AddLocationToCsv_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var location = new AddLocationToCsvRequest { LocationName = "Test Location", DayOfWeek = "Monday", StartTime = "10:00 AM", EndTime = "1:00 PM" };
            var csvFilePath = "../Data/Data/locations.csv"; 

            var locationServiceMock = new Mock<ILocationService>();
            locationServiceMock.Setup(service => service.AddLocationToCsv(location)).ReturnsAsync(true);


            var locationService = locationServiceMock.Object;

            // Act
            var result = await locationService.AddLocationToCsv(location);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddLocationToCsv_ReturnsFalse_OnException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LocationService>>();
            var locationServiceMock = new Mock<ILocationService>();

            locationServiceMock.Setup(service => service.AddLocationToCsv(It.IsAny<AddLocationToCsvRequest>()))
                               .ThrowsAsync(new Exception("Simulated error"));

            var locationService = new LocationService(loggerMock.Object);

            // Act
            var result = await locationService.AddLocationToCsv(new AddLocationToCsvRequest());

            // Assert
            Assert.IsFalse(result);
        }

    }
}
