using MyApp.Core.Commands.Contracts;
using System;

namespace MyApp.Core.Commands
{
    class EndCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Environment.Exit(0);
            return "";
        }
    }
}
