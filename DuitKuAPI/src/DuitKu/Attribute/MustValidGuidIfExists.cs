using System.ComponentModel.DataAnnotations;

namespace DuitKu.Attribute
{
    public class MustValidGuidIfExists : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value == null)
            {
                return true;
            }

            if (value is Guid guidValue)
            {
                return guidValue != Guid.Empty;
            }

            return false;
        }
    }
}