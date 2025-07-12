using DocumentProcessor.DataLayer.Interface;

namespace DocumentProcessor.Endpoints
{
    public static class LocationEndpoint
    {
        public static void AddLocationEndpoints(this WebApplication app)
        {
            app.MapGet("/location", GetLocation);
        }

        public async static Task<IResult> GetLocation(ILocationDL LocationDL)
        {
            return Results.Ok(await LocationDL.GetLocation());
        }

    }
}