using System;
using System.Collections.Generic;
using System.Text;
using CustomAutomapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;

namespace MyApp.Core.Commands
{
    public class EmployeeInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public EmployeeInfoCommand(MyAppContext context, Mapper mapper)
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

            EmployeeDto employeeDto = mapper.CreateMappedObject<EmployeeDto>(employee);

            return $"ID: {employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} -  ${employeeDto.Salary:f2}";
        }
    }
}
