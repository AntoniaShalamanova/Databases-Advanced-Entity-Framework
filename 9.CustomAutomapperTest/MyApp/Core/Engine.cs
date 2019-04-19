using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Contracts;
using System;

namespace MyApp.Core
{
    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            while (true)
            {
                string[] inputArgs = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    var commandInterpreter = this.serviceProvider.GetService<ICommandInterpreter>();

                    Console.WriteLine(commandInterpreter.Read(inputArgs));
                    Console.WriteLine();
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                }
            }
        }
    }
}
