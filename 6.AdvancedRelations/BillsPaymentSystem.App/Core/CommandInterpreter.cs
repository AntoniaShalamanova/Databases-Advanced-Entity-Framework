using System;
using System.Linq;
using System.Reflection;
using BillsPaymentSystem.App.Core.Contracts;
using BillsPaymentSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BillsPaymentSystem.App.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private const string suffix = "Command";

        public string Read(string[] args, BillsPaymentSystemContext context)
        {
            string command = args[0];

            var commandArgs = args.Skip(1).ToArray();

            var commandType = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == command + suffix);

            if (commandType == null)
            {
                throw new ArgumentNullException("Command not found!");
            }

            var commandInstance = Activator.CreateInstance(commandType, context);

            string result = ((ICommand)commandInstance).Execute(commandArgs);

            return result;
        }
    }
}
