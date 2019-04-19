using CustomAutomapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class OlderThanCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public OlderThanCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] args)
        {
            int age = int.Parse(args[0]);

            var employees = this.context
                .Employees
                .Include(e => e.Manager)
                .Where(e => (DateTime.Now - e.Birthday.Value).Days / 365.2425 > age)
                .Select(e => this.mapper.CreateMappedObject<ProjectionDto>(e))
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                string manager = emp.ManagerId != null ?
                    this.context.Employees.Find(emp.ManagerId).LastName :
                    "[no manager]";

                sb.AppendLine($"{emp.FirstName} {emp.LastName} - ${emp.Salary:F2} - Manager: {manager}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
