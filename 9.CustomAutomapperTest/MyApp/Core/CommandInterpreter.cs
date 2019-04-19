using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace MyApp.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private readonly IServiceProvider serviceProvider;
        private const string Suffix = "Command";

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string Read(string[] inputArgs)
        {
            string command = inputArgs[0] + Suffix;
            string[] commandParams = inputArgs.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == command);

            if (type == null)
            {
                throw new ArgumentNullException("Invalid command!");
            }

            var constructor = type.GetConstructors()
                .FirstOrDefault();

            var constructorParams = constructor
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();

            var services = constructorParams
                .Select(this.serviceProvider.GetService)
                .ToArray();

            ICommand cmdInstance = (ICommand)constructor.Invoke(services);

            string result = cmdInstance.Execute(commandParams);

            return result;
        }
    }
}
