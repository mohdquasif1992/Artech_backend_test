using Business.Requests;
using Business.Responses;

namespace Business.Interfaces
{
    public interface ILocationService
    {
        Task<List<LocationResponse>> GetLocationsFromCsv();
        Task<bool> AddLocationToCsv(AddLocationToCsvRequest request);
    }
}
