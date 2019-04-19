using BillsPaymentSystem.App.Core.Contracts;
using System;
using BillsPaymentSystem.Data;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class ExitCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public ExitCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            Environment.Exit(0);

            return "";
        }
    }
}
