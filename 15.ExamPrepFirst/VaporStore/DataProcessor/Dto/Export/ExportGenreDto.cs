using Newtonsoft.Json;

namespace VaporStore.DataProcessor.Dto.Export
{
    public class ExportGenreDto
    {
        [JsonProperty(PropertyName = "Id")]
        public int GenreId { get; set; }

        [JsonProperty(PropertyName = "Genre")]
        public string GenreName { get; set; }

        public ExportGameDto[] Games { get; set; }

        public int TotalPlayers { get; set; }
    }
}
