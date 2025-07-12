using Microsoft.Extensions.Options;

namespace DocumentProcessor.Startup
{
    public static class CorsConfig
    {
        public static void AddCors(this WebApplicationBuilder builder, string[] allowedUrls)
        {            
            builder.Services.AddCors(
                options =>
                {
                    options.AddPolicy("CorsPolicy", policy =>
                    {
                        policy.WithOrigins(allowedUrls).AllowAnyHeader().AllowAnyMethod();
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                        policy.AllowCredentials();
                    });
                });
        }
    }
}
