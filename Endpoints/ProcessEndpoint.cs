using DocumentProcessor.DataLayer.Interface;

namespace DocumentProcessor.Endpoints
{
    public static class ProcessEndpoint
    {
        public static void AddProcessEndpoints(this WebApplication app)
        {
            app.MapGet("/process", GetProcess);
        }

        public async static Task<IResult> GetProcess(IProcessDL processDL)
        {            
            return Results.Ok(await processDL.GetProcess());
        }

    }
}
