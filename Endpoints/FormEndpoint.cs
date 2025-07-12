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
            app.MapGet("/form", GetForm).DisableAntiforgery();
            app.MapPost("/form", PostForm).DisableAntiforgery().RequireAuthorization();
        }
        public static async Task<IResult> GetForm(IFormDL formDL, [AsParameters]QueryFilter<FormResponse> filter)            
        {
            var validationError = filter.Validate(typeof(FormResponse));
            if(validationError != null && validationError.Any())
            {
                return Results.BadRequest(validationError);
            }
            var response = await formDL.GetForm(filter);
            return Results.Ok(response);
        }

        public static async Task<IResult> PostForm(IFormDL formDL, [FromForm]string request, IFormFileCollection attachments, IValidator<IFormFile> attachmentValidator)
        {
            var jsonSerialiserOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var formRequest = System.Text.Json.JsonSerializer.Deserialize<FormRequest>(request, jsonSerialiserOptions);            
            var error = ValidatePostForm(formRequest?.Form, attachments, attachmentValidator);
            if (error != "")
            {
                return Results.BadRequest(new ValidationModel(error));
            }

            var response = await formDL.PostForm(formRequest?.Form, attachments, formRequest?.DeleteAttachments ?? []);
            if (response == 0)
            {
                return Results.BadRequest("Failed to add form data");
            }
            string successMessage;
            if(formRequest.Form.Id == 0)
            {
                successMessage = $"Form:{response} has been created";
            }
            else if(formRequest.Form.StatusId == 4)
            {
                successMessage = $"Form:{response} has been completed";
            }
            else
            {
                successMessage = $"Form:{response} has been updated";
            }
            return Results.Ok(successMessage);
        }
        public static string ValidatePostForm(Form? form, IFormFileCollection? attachments, IValidator<IFormFile> attachmentValidator)
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
            foreach (var attachment in attachments)
            {   
                if(attachment == null)
                {
                    return "Attachment is null";
                }
                error = attachmentValidator.Validate(attachment).ToString().Split("\n")[0];
                if (error != "")
                {
                    return error;
                }
            }                                 
            return string.Empty;
        }
    }
}
