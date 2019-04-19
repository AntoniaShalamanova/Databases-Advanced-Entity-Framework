using Newtonsoft.Json;

namespace VaporStore.DataProcessor.Dto.Export
{
    public class ExportGameDto
    {
        [JsonProperty(PropertyName = "Id")]
        public int GameId { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string GameName { get; set; }

        [JsonProperty(PropertyName = "Developer")]
        public string DeveloperName { get; set; }

        [JsonProperty(PropertyName = "Tags")]
        public string TagNames { get; set; }

        [JsonProperty(PropertyName = "Players")]
        public int PlayersCount { get; set; }
    }
}
