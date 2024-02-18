using CsvHelper.Configuration.Attributes;

namespace Business.Responses
{
    public class LocationResponse
    {
        [Index(0)]
        public int LocationId { get; set; }

        [Index(1)]
        public string? LocationName { get; set; }

        [Index(2)]
        public string? DayOfWeek { get; set; }

        [Index(3)]
        public string? StartTime { get; set; }

        [Index(4)]
        public string? EndTime { get; set; }
    }
}
