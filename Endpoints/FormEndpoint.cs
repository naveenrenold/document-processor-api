using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;
using DocumentProcessor.Validator.Model;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net.WebSockets;

namespace DocumentProcessor.Endpoints
{
    public static class FormEndpoint
    {
        public static void AddFormEndpoints(this WebApplication app)
        {
            app.MapGet("/form", GetForm);
            app.MapPost("/form", PostForm).DisableAntiforgery();
        }
        public static async Task<IResult> GetForm(IFormDL formDL, [AsParameters]QueryFilter filter)
        {
            var response = await formDL.GetForm(filter);
            return Results.Ok(response);
        }

        public static async Task<IResult> PostForm(IFormDL formDL, [FromForm]string request, IFormFileCollection attachments)
        {
            var form = System.Text.Json.JsonSerializer.Deserialize<Form>(request, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var deleteAttachments = System.Text.Json.JsonSerializer.Deserialize<DeleteAttachmentsRequest>(request, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var error = ValidatePostForm(form, attachments);
            if (error != "")
            {
                return Results.BadRequest(new ValidationModel(error));
            }

            var response = await formDL.PostForm(form, attachments, deleteAttachments?.DeleteAttachments ?? []);
            if (response == 0)
            {
                return Results.BadRequest("Failed to add form data");
            }            
            return Results.Ok($"Form:{response} has been created");
        }
        public static string ValidatePostForm(Form? form, IFormFileCollection? attachments)
        {
            if(form == null)
            {
                return "Form data is null";
            }
            var formValidator = new FormValidator();
            var error = formValidator.Validate(form).ToString().Split("\n")[0];
            if (error != "")
            {
                return error;
            }
            if(attachments == null || attachments.Count == 0)
            {
                return string.Empty;
            }
            var attachmentsValidator = new AttachmentValidator();
            foreach (var attachment in attachments)
            {   
                if(attachment == null)
                {
                    return "Attachment is null";
                }
                error = attachmentsValidator.Validate(attachment).ToString().Split("\n")[0];
                if (error != "")
                {
                    return error;
                }
            }                                 
            return string.Empty;
        }
    }
}
