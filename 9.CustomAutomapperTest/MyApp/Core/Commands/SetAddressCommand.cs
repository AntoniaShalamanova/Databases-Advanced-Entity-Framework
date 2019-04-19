using CustomAutomapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Linq;

namespace MyApp.Core.Commands
{
    public class SetAddressCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public SetAddressCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            args = args.Skip(1).ToArray();

            string address = string.Join(" ", args);

            var employee = this.context
                .Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException("Employee does not exist!");
            }

            employee.Address = address;
            context.SaveChanges();

            AddressDto addressDto = mapper.CreateMappedObject<AddressDto>(employee);

            return $"Address: {addressDto.Address} set successfully to employee with id {addressDto.Id}!";
        }
    }
}
