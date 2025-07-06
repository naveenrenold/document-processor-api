using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DocumentProcessor.Startup
{
    public static class Authentication
    {

        public static void AddAuthenticationDI(this WebApplicationBuilder builder)
        {
            //JwtBearerDefaults.AuthenticationScheme
            builder.Services.AddAuthentication().AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {                        
                        ValidIssuer = "https://sts.windows.net/24d2489e-7bb3-4339-94a2-207bb2a75abc/",                       
                        ValidAudience = "api://172d1695-b52e-45ae-88ae-acbbad8c34ee",                                                
                        ValidateIssuerSigningKey = false,
                        RequireSignedTokens = false
                    };
                }
                );
        }

        public static void AddAuthourizationDI(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization();
        }
    }
}
