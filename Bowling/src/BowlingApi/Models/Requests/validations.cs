using BowlingApi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingApi.Models.Requests
{
    public class validations
    {
    }

    public class GameIdCheckAttribute : ValidationAttribute
    {
        private IBowlingService _service;

        public GameIdCheckAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Int32.TryParse(Convert.ToString(value), out var id);
            _service = (IBowlingService)validationContext.GetService(typeof(IBowlingService));
            if (_service.IsGameIdValid(id))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Game Id is invalid");
        }
    }

    public class FoulCheckAttribute : ValidationAttribute
    {
        public FoulCheckAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var foul = Convert.ToChar(value);
            var validValues = new List<char> { 'F', 'Y', 'N', '\0' };
            if (validValues.Contains(char.ToUpper(foul)))
                return true;

            return false;
        }
    }
}
