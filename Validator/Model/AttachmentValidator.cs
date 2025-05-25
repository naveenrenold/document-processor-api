using DocumentProcessor.Model;
using FluentValidation;

namespace DocumentProcessor.Validator.Model
{
    public class AttachmentsValidator : AbstractValidator<List<Attachment>>
    {
        public AttachmentsValidator()
        {
            RuleForEach(attachment => attachment).SetValidator(new AttachmentValidator());
            RuleForEach(attachment => attachment).SetValidator(new AttachmentValidator());
        }
    }

    public class AttachmentValidator : AbstractValidator<Attachment>
    {
        public AttachmentValidator()
        {
            RuleFor(attachment => attachment.FileName)
                .NotEmpty().WithMessage("File Name is required.")
                .MaximumLength(100).WithMessage("File Name cannot exceed 100 characters.");
            RuleFor(attachment => attachment.UploadedBy)
                .NotEmpty().WithMessage("Uploaded By is required.")
                .MaximumLength(100).WithMessage("Uploaded By cannot exceed 100 characters.");
        }
    }
}
