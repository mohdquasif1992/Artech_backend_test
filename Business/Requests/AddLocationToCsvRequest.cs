using System.ComponentModel;
using CsvHelper.Configuration.Attributes;

namespace Business.Requests
{
    public class AddLocationToCsvRequest
    {
        [Index(0)]
        [DefaultValue("1")]
        public int LocationId { get; set; }

        [Index(1)]
        [DefaultValue("Bakery Shop")]
        public string? LocationName { get; set; }

        [Index(2)]
        [DefaultValue("Monday")]
        public string? DayOfWeek { get; set; }

        [Index(3)]
        [DefaultValue("10:00")]
        public string? StartTime { get; set; }

        [Index(4)]
        [DefaultValue("13:00")]
        public string? EndTime { get; set; }
    }
}
