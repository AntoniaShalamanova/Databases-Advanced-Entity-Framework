using BillsPaymentSystem.App.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using BillsPaymentSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class UserInfoCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public UserInfoCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            int userId = int.Parse(args[0]);

            //User user = this.context
            //    .Users
            //    .Include(u => u.PaymentMethods)
            //    .ThenInclude(pm => pm.CreditCard)
            //    .Include(u => u.PaymentMethods)
            //    .ThenInclude(pm => pm.BankAccount)
            //    .FirstOrDefault(u => u.UserId == userId);

            var user = this.context
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


            if (user == null)
            {
                throw new ArgumentNullException($"User with id {userId} not found!");
            }

            sb.AppendLine($"User: {user.FullName}");

            sb.AppendLine("Bank Accounts:");
            foreach (var bankAccount in user.BankAccounts.OrderBy(ba => ba.BankAccountId))
            {
                sb.AppendLine($"-- ID: {bankAccount.BankAccountId}");
                sb.AppendLine($"--- Balance: {bankAccount.Balance}");
                sb.AppendLine($"--- Bank: {bankAccount.BankName}");
                sb.AppendLine($"--- SWIFT: {bankAccount.SwiftCode}");
            }

            sb.AppendLine("Credit Cards:");
            foreach (var creditCard in user.CreditCards.OrderBy(cc => cc.CreditCardId))
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
