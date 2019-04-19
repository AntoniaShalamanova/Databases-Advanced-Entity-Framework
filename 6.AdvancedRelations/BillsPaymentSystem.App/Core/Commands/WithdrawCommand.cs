using BillsPaymentSystem.App.Core.Contracts;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class WithdrawCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public WithdrawCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            int userId = int.Parse(args[0]);
            decimal amount = decimal.Parse(args[1]);

            var user = this.context
                .Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentNullException($"User with id {userId} not found!");
            }

            var userBankAccounts = user.PaymentMethods
                .Where(pm => pm.Type == PaymentType.BankAccount)
                .Select(pm => pm.BankAccount)
                .ToArray();

            foreach (var bankAccount in userBankAccounts)
            {
                if (bankAccount.Balance-amount<0)
                {
                    Console.WriteLine("Insufficient funds!");
                    continue;
                }

                bankAccount.Balance -= amount;
            }

            context.SaveChanges();

            var userInfo = this.context
                .Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    u.UserId,
                    FullName = u.FirstName + " " + u.LastName,
                    CreditCards = u.PaymentMethods
                        .Where(pm => pm.Type == PaymentType.CreditCard)
                        .Select(pm => pm.CreditCard)
                        .ToArray(),
                    BankAccounts = u.PaymentMethods
                        .Where(pm => pm.Type == PaymentType.BankAccount)
                        .Select(pm => pm.BankAccount)
                        .ToArray()
                })
                .FirstOrDefault();

            sb.AppendLine($"User: {userInfo.FullName}");

            sb.AppendLine("Bank Accounts:");
            foreach (var bankAccount in userInfo.BankAccounts.OrderBy(ba => ba.BankAccountId))
            {
                sb.AppendLine($"-- ID: {bankAccount.BankAccountId}");
                sb.AppendLine($"--- Balance: {bankAccount.Balance}");
                sb.AppendLine($"--- Bank: {bankAccount.BankName}");
                sb.AppendLine($"--- SWIFT: {bankAccount.SwiftCode}");
            }

            sb.AppendLine("Credit Cards:");
            foreach (var creditCard in userInfo.CreditCards.OrderBy(cc => cc.CreditCardId))
            {
                sb.AppendLine($"-- ID: {creditCard.CreditCardId}");
                sb.AppendLine($"--- Limit: {creditCard.Limit}");
                sb.AppendLine($"--- Money Owed: {creditCard.MoneyOwed}");
                sb.AppendLine($"--- Limit Left: {creditCard.LimitLeft}");
                sb.AppendLine($"--- Expiration Date: {creditCard.ExpirationDate}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
