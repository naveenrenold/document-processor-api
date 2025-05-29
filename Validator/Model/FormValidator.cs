using DocumentProcessor.Model;
using FluentValidation;

namespace DocumentProcessor.Validator.Model
{
    public class FormValidator : AbstractValidator<Form>
    {
        public FormValidator()
        {
            RuleFor(form => form.CustomerName)
                .NotEmpty().WithMessage("Customer Name is required.")
                .MaximumLength(100).WithMessage("Customer Name cannot exceed 100 characters.");
            RuleFor(form => form.CustomerAddress)
                .NotEmpty().WithMessage("Customer Address is required.")
                .MaximumLength(500).WithMessage("Customer Address cannot exceed 500 characters.");
            RuleFor(form => form.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(100).WithMessage("Location cannot be greater than 100 characters");
            RuleFor(form => form.LastUpdatedBy)
                .NotEmpty().WithMessage("Updated by user is empty.")
                .MaximumLength(100).WithMessage("Updated username cannot exceed 100 characters.");
            RuleFor(form => form.ProcessId)
                .NotEmpty().WithMessage("Process is required.")
                .GreaterThan(0).WithMessage("Process must be greater than 0.");
            RuleFor(form => form.PhoneNumber)
                .Matches("^(\\+91|\\+91-|0)?[789]\\d{9}$").WithMessage("Phone Number is not valid.")
                .NotEmpty().WithMessage("Phone Number is required.")
                .MaximumLength(15).WithMessage("Phone Number cannot exceed 15 characters.");
            RuleFor(form => form.PhoneNumber2)
                .Matches("^(\\+91|\\+91-|0)?[789]\\d{9}$").When(form => !string.IsNullOrEmpty(form.PhoneNumber2)).WithMessage("Phone Number is not valid.")                
                .MaximumLength(15).When(form => !string.IsNullOrEmpty(form.PhoneNumber2)).WithMessage("Phone Number2 cannot exceed 15 characters.");
        }
    }
}
