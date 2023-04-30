using FluentValidation;
using FluentValidation.Results;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        // static class icerisinde non-static method yazmaq olmaz !
        public static void Validate(IValidator validator, object obj)
        {
            ValidationContext<Object> context = new ValidationContext<Object>(obj);
            ValidationResult result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
