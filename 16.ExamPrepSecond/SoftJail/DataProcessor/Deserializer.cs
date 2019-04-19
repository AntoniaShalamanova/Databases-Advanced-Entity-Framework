using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using SoftJail.DataProcessor.ImportDto;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();

            List<Department> departmentsToInsert = new List<Department>();

            Department[] departments = JsonConvert.DeserializeObject<Department[]>(jsonString);

            foreach (var department in departments)
            {
                bool isValidCells = true;

                if (!IsValid(department) || department.Cells.Count == 0)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                foreach (var cell in department.Cells)
                {
                    if (!IsValid(cell))
                    {
                        isValidCells = false;
                        break;
                    }
                }

                if (!isValidCells)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                departmentsToInsert.Add(department);
                result.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departmentsToInsert);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();

            List<Prisoner> prisonersToInsert = new List<Prisoner>();

            var settings = new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy",
                Culture = CultureInfo.InvariantCulture
            };

            Prisoner[] prisoners = JsonConvert.DeserializeObject<Prisoner[]>(jsonString, settings);

            foreach (var prisoner in prisoners)
            {
                bool isValidCells = true;

                if (!IsValid(prisoner) || prisoner.Mails.Count == 0)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                foreach (var mail in prisoner.Mails)
                {
                    if (!IsValid(mail))
                    {
                        isValidCells = false;
                        break;
                    }
                }

                if (!isValidCells)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                prisonersToInsert.Add(prisoner);
                result.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisonersToInsert);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportOfficerDto[]), new XmlRootAttribute("Officers"));

            var officersDto = (ImportOfficerDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder result = new StringBuilder();

            var officers = new List<Officer>();

            foreach (var officerDto in officersDto)
            {
                var isValidPosition = Enum.TryParse<Position>(officerDto.Position, out Position position);
                var isValidWeapon = Enum.TryParse<Weapon>(officerDto.Weapon, out Weapon weapon);

                if (!IsValid(officerDto) || !isValidPosition || !isValidWeapon || officerDto.Prisoners.Length == 0)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = officerDto.FullName,
                    Salary = officerDto.Salary,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = officerDto.DepartmentId
                };

                List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    var prisoner = context.Prisoners.Find(prisonerDto.Id);

                    officerPrisoners.Add(new OfficerPrisoner()
                    {
                        Prisoner = prisoner,
                        Officer = officer
                    });
                }

                officer.OfficerPrisoners = officerPrisoners;

                officers.Add(officer);
                result.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}