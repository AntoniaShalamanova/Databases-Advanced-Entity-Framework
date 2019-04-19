using CustomAutomapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Globalization;

namespace MyApp.Core.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public SetBirthdayCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);
            DateTime date = DateTime.ParseExact(args[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var employee = this.context
                .Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException("Employee does not exist!");
            }

            employee.Birthday = date;
            context.SaveChanges();

            BirthdayDto birthdayDto = mapper.CreateMappedObject<BirthdayDto>(employee);

            return $"Date: {birthdayDto.Birthday.ToString("dd-MM-yyyy")} set successfully to employee with id {birthdayDto.Id}!";
        }
    }
}
