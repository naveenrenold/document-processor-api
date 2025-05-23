namespace DocumentProcessor.Endpoints
{
public static class RootEndpoint
{    
    public static void AddEndpoints(this WebApplication app)
    {
      var formEndpoint = new FormEndpoint();
      app.MapGet("/form", formEndpoint.GetForm);
    }
}    
}
