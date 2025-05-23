

using DocumentProcessor.DataLayer.Interface;

namespace DocumentProcessor.Endpoints
{
    public class FormEndpoint(IFormDL formDL)
    {
        public IResult GetForm()
        {
            var response = formDL.GetForm();
            return Results.Ok(response);
        }

        public IResult PostForm()
        {
            var response = formDL.PostForm();
            if(response)
            {
                return Results.Ok();
            }
            return Results.BadRequest("Failed to add form data");
        }
    }
}
