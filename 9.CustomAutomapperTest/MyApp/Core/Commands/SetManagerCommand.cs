using MyApp.Core.Commands.Contracts;
using MyApp.Data;
using System;

namespace MyApp.Core.Commands
{
    public class SetManagerCommand : ICommand
    {
        private readonly MyAppContext context;

        public SetManagerCommand(MyAppContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            int employeeId =int.Parse(args[0]);
            int managerId = int.Parse(args[1]);

            var employee = this.context.Employees.Find(employeeId);
            var manager = this.context.Employees.Find(managerId);

            if (employee == null || manager == null)
            {
                throw new ArgumentNullException("Employee or manager does not exist!");
            }

            employee.Manager = manager;
            context.SaveChanges();

            return $"Person with id {manager.Id} was set successfully to manage employee with id {employee.Id}!";
        }
    }
}
