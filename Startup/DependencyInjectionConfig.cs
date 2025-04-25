

namespace DocumentProcessor.Startup
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Scan(scan => scan
            .FromAssemblyOf<Program>() // or any known type in the same assembly
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))            
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        }        
    }
}
