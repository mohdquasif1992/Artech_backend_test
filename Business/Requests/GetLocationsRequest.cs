using System.ComponentModel;

namespace Business.Requests
{
    public class GetLocationsRequest
    {
        [DefaultValue("10:00")]
        public string? StartTime { get; set; }

       
        [DefaultValue("13:00")]
        public string? EndTime { get; set; }
    }
}
