using System;
using System.ComponentModel.DataAnnotations;
using BillsPaymentSystem.Models.Attributes;

namespace BillsPaymentSystem.Models
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Limit { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal MoneyOwed { get; set; }

        //Calculated properties are not included in the database!
        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        [ExpirationDate]
        public DateTime ExpirationDate { get; set; }

        //No need Id for one-to-one relationship
        public PaymentMethod PaymentMethod { get; set; }
    }
}
