using Enum;
using System.Text.Json.Serialization;

namespace Portfolio.Model.Data
{
    public class DataHistory
    {
        public string Id { get; set; }
        public string ElementId { get; set; }
        public string ElementName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PortfolioElementEnum ElementType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ObjectHistoryEnum Action { get; set; }

        public int CreationDate { get; set; }

    }
}