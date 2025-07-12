using DocumentProcessor.Endpoints;
using DocumentProcessor.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.AddDependencies();
builder.AddCors(builder.Configuration.GetSection("AppSettings:AllowedUrls").Get<string[]>() ?? []);
builder.AddSqlConnection(builder.Configuration.GetConnectionString("Database") ?? "");
builder.AddFtpConnection(builder.Configuration.GetSection("AppSettings:FTPServer").Get<string>() ?? "", builder.Configuration.GetSection("AppSettings:FTPUsername").Get<string>() ?? "", builder.Configuration.GetSection("AppSettings:FTPPassword").Get<string>() ?? "");
builder.AddAuthenticationDI();
builder.AddAuthourizationDI();


var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.AddFormEndpoints();
app.AddProcessEndpoints();
app.AddAttachmentEndpoints();
app.AddActivityEndpoints();
app.AddLocationEndpoints();


app.Run();

