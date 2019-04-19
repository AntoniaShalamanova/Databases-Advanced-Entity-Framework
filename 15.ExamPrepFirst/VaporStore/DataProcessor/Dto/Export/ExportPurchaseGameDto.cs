using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Game")]
    public class ExportPurchaseGameDto
    {
        [XmlAttribute("title")]
        public string GameName { get; set; }

        [XmlElement("Genre")]
        public string GenreName { get; set; }

        [XmlElement("Price")]
        public decimal GamePrice { get; set; }
    }
}
