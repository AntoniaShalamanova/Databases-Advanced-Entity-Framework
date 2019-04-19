using CustomAutomapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Text;

namespace MyApp.Core.Commands
{
    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public EmployeePersonalInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var employee = this.context
                .Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException("Employee does not exist!");
            }

            EmployeePersonalInfoDto employeeDto = mapper.CreateMappedObject<EmployeePersonalInfoDto>(employee);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"ID: {employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:F2}");
            sb.AppendLine($"Birthday: {employeeDto.Birthday?.ToString("dd-MM-yyyy")}");
            sb.AppendLine($"Address: {employeeDto.Address}");

            return sb.ToString().TrimEnd();
        }
    }
}
