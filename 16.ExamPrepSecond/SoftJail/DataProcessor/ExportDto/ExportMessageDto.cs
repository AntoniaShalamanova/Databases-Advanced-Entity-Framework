using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Message")]
    public class ExportMessageDto
    {
        [XmlElement]
        public string Description { get; set; }
    }
}
