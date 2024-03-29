﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpirationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentDateTime = DateTime.Now;
            var targetDateTime = (DateTime) value;

            if (currentDateTime > targetDateTime)
            {
                return  new ValidationResult("Card is expired!");
            }

            return  ValidationResult.Success;
        }
    }
}
