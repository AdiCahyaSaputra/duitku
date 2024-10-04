using System.ComponentModel.DataAnnotations;

namespace DuitKu.Attribute
{
    public class RequiredMin : ValidationAttribute
    {
        public double MinimalValue { get; }

        public RequiredMin(double value)
        {
            MinimalValue = value;
        }

        public override bool IsValid(object? value)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue > (decimal)MinimalValue;
            }
            return false;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (!IsValid(value))
            {
                return new ValidationResult(ErrorMessage ?? $"Jumlah desimal wajib di isi dan harus lebih dari {MinimalValue}");
            }

            return ValidationResult.Success!;
        }
    }
}