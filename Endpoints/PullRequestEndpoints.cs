

namespace GitReminder.Endpoints
{
    public static class PullRequestEndpoints
    {
        public static void AddPullRequestEndpoints(this WebApplication app)
        {
            app.MapGet("/pr", GetPullRequests);
        }
        public static IResult GetPullRequests()
        {
            
            return Results.Ok("Hello World");
        }
    }
}
