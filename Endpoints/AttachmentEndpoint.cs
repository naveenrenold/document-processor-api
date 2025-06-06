using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;
using Microsoft.CodeAnalysis;

namespace DocumentProcessor.Endpoints
{
    public static class AttachmentEndpoint
    {
        public static void AddAttachmentEndpoints(this WebApplication app)
        {
            app.MapGet("/attachment", GetAttachment);
            //app.MapPost("/attachment", PostAttachment).DisableAntiforgery();
        }
        public static async Task<IResult> GetAttachment(IAttachmentDL attachmentDL, [AsParameters] QueryFilter<AttachmentResponse> filter)
        {
            var validationError = filter.Validate(typeof(AttachmentResponse));
            if (validationError != null && validationError.Any())
            {
                return Results.BadRequest(validationError);
            }
            var response = await attachmentDL.GetAttachment(filter);
            return Results.Ok(response);
        }
        //public static async Task<IResult> PostAttachment(IAttachmentDL attachmentDL, [AsParameters] QueryFilter<AttachmentResponse> filter)
        //{
        //    var response = await attachmentDL.GetAttachment(filter);
        //    return Results.Ok(response);
        //}
    }
}
