using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SoftJail.DataProcessor.ExportDto;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .ToArray();

            var prisonersObjs = prisoners.Select(p => new
            {
                Id = p.Id,
                Name = p.FullName,
                CellNumber = p.Cell.CellNumber,
                Officers = p.PrisonerOfficers.Select(po => new
                {
                    OfficerName = po.Officer.FullName,
                    Department = po.Officer.Department.Name
                })
                    .OrderBy(o => o.OfficerName)
                    .ToArray(),
                TotalOfficerSalary = decimal.Parse(p.PrisonerOfficers.Sum(po => po.Officer.Salary).ToString("F2"))
            })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(prisonersObjs, Formatting.Indented);

            return jsonString;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] splitedPrisonersNames = prisonersNames
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var prisoners = context.Prisoners
                .Where(p => splitedPrisonersNames.Contains(p.FullName))
                .ToArray();

            var prisonersDto = prisoners.Select(p => new ExportPrisonerDto
            {
                Id = p.Id,
                FullName = p.FullName,
                IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                EncryptedMessages = p.Mails.Select(m => new ExportMessageDto()
                {
                    Description = string.Join("", m.Description.Reverse())
                })
                    .ToArray()
            })
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportPrisonerDto[]), new XmlRootAttribute("Prisoners"));

            StringBuilder result = new StringBuilder();

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(new StringWriter(result), prisonersDto, namespaces);

            return result.ToString().TrimEnd();
        }
    }
}