﻿using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExportPrisonerDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string FullName { get; set; }

        [XmlElement]
        public string IncarcerationDate { get; set; }

        [XmlArray]
        public ExportMessageDto[] EncryptedMessages { get; set; }
    }
}
