using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;
using DocumentProcessor.Validator.Model;
using FluentValidation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DocumentProcessor.Endpoints
{
    public static class FormEndpoint
    {
        public static void AddFormEndpoints(this WebApplication app)
        {
            app.MapGet("/form", GetForm);
            app.MapPost("/form", PostForm);
        }
        public static async Task<IResult> GetForm(IFormDL formDL)
        {
            var response = await formDL.GetForm();
            return Results.Ok(response);
        }

        public static async Task<IResult> PostForm(IFormDL formDL, FormRequest request)
        {
            var error = ValidatePostForm(request);
            if(error != "")
            {
                return Results.BadRequest(new ValidationModel(error));
            }

            var response = await formDL.PostForm(request.Form, request.Attachments);
            if(response > 0)
            {
                return Results.Ok($"Form:{response} has been created");
            }
            return Results.BadRequest("Failed to add form data");
        }
        public static string ValidatePostForm(FormRequest request)
        {            
            var formValidator = new FormValidator();
            var error = formValidator.Validate(request.Form).ToString().Split("\n")[0];
            if (error != "" )
            {
                return error;
            }
            var attachmentsValidator = new AttachmentsValidator();
            if(request.Attachments != null && request.Attachments.Count != 0)
            {
               return attachmentsValidator.Validate(request.Attachments).ToString().Split("\n")[0];
            }
            return string.Empty;

        }
    }
}
