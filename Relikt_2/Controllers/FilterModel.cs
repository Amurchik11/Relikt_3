using System.Text.Json.Serialization;

namespace Relikt_2.Controllers
{
    public class FilterModel
    {
        [JsonPropertyName("property")]
        public string PropertyName { get; set; }

        [JsonPropertyName("Value")]
        public int Value { get; set; }
    }
}
