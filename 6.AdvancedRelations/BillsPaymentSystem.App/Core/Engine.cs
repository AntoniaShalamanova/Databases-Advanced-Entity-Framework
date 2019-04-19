using BillsPaymentSystem.App.Core.Contracts;
using System;
using BillsPaymentSystem.Data;

namespace BillsPaymentSystem.App.Core
{
    public class Engine : IEngine
    {
        private readonly ICommandInterpreter commandInterpreter;

        public Engine(ICommandInterpreter commandInterpreter)
        {
            this.commandInterpreter = commandInterpreter;
        }

        public void Run()
        {
            Console.WriteLine("Insert one of the following commands [UserInfo/Deposit/Withdraw/PayBills/Exit]: ");
            var input = Console.ReadLine();

            while (true)
            {
                string result;

                string[] commandArgs = input
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

                using (var context = new BillsPaymentSystemContext())
                {
                    try
                    {
                        result = this.commandInterpreter.Read(commandArgs, context);

                        Console.WriteLine(result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Insert one of the following commands [UserInfo/Deposit/Withdraw/PayBills/Exit]: ");
                input = Console.ReadLine();
            }
        }
    }
}
