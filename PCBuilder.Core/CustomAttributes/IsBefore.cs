using System.ComponentModel.DataAnnotations;

namespace PCBuilder.Core.CustomAttributes
{
    public class IsBefore : ValidationAttribute
    {
        private readonly string propertyToCompare;

        public IsBefore(string _propertyToCompare, string errorMessage = "")
        {
            propertyToCompare = _propertyToCompare;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                DateTime datetoCompare = (DateTime)validationContext
                .ObjectType
                .GetProperty(propertyToCompare)
                .GetValue(validationContext.ObjectInstance);

                if ((DateTime)value < datetoCompare)
                {
                    return ValidationResult.Success;
                }
            }
            catch (Exception)
            {}

            return new ValidationResult(ErrorMessage);
        }
    }
}
