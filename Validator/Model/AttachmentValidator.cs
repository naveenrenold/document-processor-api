using DocumentProcessor.Model;
using FluentValidation;

namespace DocumentProcessor.Validator.Model
{
    //public class AttachmentsValidator : AbstractValidator<List<FormFile>>
    //{
    //    public AttachmentsValidator()
    //    {
    //        RuleForEach(attachment => attachment).SetValidator(new AttachmentValidator());
    //        RuleForEach(attachment => attachment).SetValidator(new AttachmentValidator());
    //    }
    //}

    public class AttachmentValidator : AbstractValidator<IFormFile>
    {
        public AttachmentValidator()
        {
            RuleFor(attachment => attachment.FileName)
                .NotEmpty().WithMessage("File Name is required.")
                .MaximumLength(100).WithMessage("File Name cannot exceed 100 characters.");
            RuleFor(attachment => attachment.Length/1024)
                .NotEmpty().WithMessage("File is empty.")
                .LessThan(2000).WithMessage("File size must be less than 2000 Kb");
            RuleFor(RuleFor => RuleFor.ContentType)
                .NotEmpty().WithMessage("Content Type is required.")
                .Must(contentType =>
                    contentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase) ||
                    contentType.Equals("image/png", StringComparison.OrdinalIgnoreCase) ||
                    contentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                    contentType.Equals("image/jpg", StringComparison.OrdinalIgnoreCase)).WithMessage("Content Type must be application/pdf, image/png, image/jpeg, or image/jpg.");

        }
    }
}
