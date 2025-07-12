using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;
using Microsoft.CodeAnalysis;

namespace DocumentProcessor.Endpoints
{
    public static class ActivityEndpoint
    {

            public static void AddActivityEndpoints(this WebApplication app)
            {
                app.MapGet("/Activity", GetActivity).RequireAuthorization();
                //app.MapPost("/Activity", PostActivity).DisableAntiforgery();
            }
            public static async Task<IResult> GetActivity(IActivityDL activityDL, [AsParameters] QueryFilter<ActivityResponse> filter)
            {
                var validationError = filter.Validate(typeof(ActivityResponse));
                if (validationError != null && validationError.Any())
                {
                    return Results.BadRequest(validationError);
                }
                var response = await activityDL.GetActivity(filter);
                return Results.Ok(response);
            }
        }
}
