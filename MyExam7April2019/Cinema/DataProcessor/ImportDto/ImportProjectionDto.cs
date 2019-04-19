using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Projection")]
    public class ImportProjectionDto
    {
        [XmlElement]
        public int MovieId { get; set; }

        [XmlElement]
        public int HallId { get; set; }

        [Required]
        [XmlElement]
        public string DateTime { get; set; }
    }
}
