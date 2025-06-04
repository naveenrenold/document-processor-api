using DocumentProcessor.Model;
using DocumentProcessor.Startup;
using FluentValidation;
using Microsoft.Extensions.Options;

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
        public AttachmentValidator(IOptions<AppSettings> appSettings)
        {
            RuleFor(attachment => attachment.FileName)
                .NotEmpty().WithMessage("File Name is required.")
                .MaximumLength(100).WithMessage("File Name cannot exceed 100 characters.");
            RuleFor(attachment => attachment.Length/1024)
                .NotEmpty().WithMessage("File is empty.")
                .LessThan(appSettings.Value.MaxFileSize).WithMessage($"File size must be less than {appSettings.Value.MaxFileSize} Kb");
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
