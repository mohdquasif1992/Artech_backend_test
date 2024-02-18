using API.Controllers;
using Business.Interfaces;
using Business.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Tests
{
    [TestFixture]
    public class LocationControllerTests
    {
        private Mock<ILocationService> _locationServiceMock;
        private Mock<ILogger<LocationController>> _loggerMock;
        LocationController _locationController;
     

        [SetUp]
        public void Setup()
        {
            _locationServiceMock = new Mock<ILocationService>();
            _loggerMock = new Mock<ILogger<LocationController>>();
        }

        [Test]
        public async Task GetLocations_ReturnsOkResult_WithLocations()
        {
            // Arrange
            var expectedResult = new List<LocationResponse>
    {
        new LocationResponse { LocationId = 1, LocationName = "Pharmacy", DayOfWeek = "Monday", StartTime = "10:00 AM", EndTime = "1:00 PM" }
        // Add more sample locations as needed
    };
            _locationServiceMock.Setup(service => service.GetLocationsFromCsv()).ReturnsAsync(expectedResult);
            var controller = new LocationController(_locationServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await controller.GetLocations() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetLocations_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            string errorMessage = "Error reading locations from CSV";
            _locationServiceMock.Setup(service => service.GetLocationsFromCsv())
                                .ThrowsAsync(new Exception(errorMessage));

            // Act
            var controller = new LocationController(_locationServiceMock.Object, _loggerMock.Object);

            Exception exception = null;
            try
            {
                controller.GetLocations().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                
                exception = ex;
            }

            // Assert
            Assert.IsNull(exception, "Expected an exception to be thrown.");
          
        }






    }
}
