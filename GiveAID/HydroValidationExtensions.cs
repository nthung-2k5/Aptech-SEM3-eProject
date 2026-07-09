using FluentValidation;
using Hydro;
using PhoneNumbers;

namespace GiveAID;

public static class HydroValidationExtensions
{
    public static bool Validate<TComponent>(this TComponent component, IValidator<TComponent> validator) where TComponent : HydroComponent
    {
        component.IsModelTouched = true;
        var result = validator.Validate(component);

        foreach (var error in result.Errors) 
        {
            component.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return result.IsValid;
    }
    
    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder, string? defaultRegion = null)
    {
        return ruleBuilder.Must(phone =>
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            var phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var numberProto = phoneUtil.Parse(phone, defaultRegion);
                
                return phoneUtil.IsValidNumber(numberProto);
            }
            catch (NumberParseException)
            {
                return false;
            }
        });
    }
}