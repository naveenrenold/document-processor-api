using DocumentProcessor.Endpoints;
using DocumentProcessor.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.AddDependencies();
builder.AddSqlConnection(builder.Configuration.GetConnectionString("Database") ?? "");
builder.AddCors(builder.Configuration.GetSection("AppSettings:AllowedUrls").Get<string[]>() ?? []);
builder.AddFtpConnection(builder.Configuration.GetSection("AppSettings:FTPServer").Get<string>() ?? "", builder.Configuration.GetSection("AppSettings:FTPUsername").Get<string>() ?? "", builder.Configuration.GetSection("AppSettings:FTPPassword").Get<string>() ?? "");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.AddFormEndpoints();
app.AddProcessEndpoints();
app.AddAttachmentEndpoints();
app.AddActivityEndpoints();
app.UseCors("CorsPolicy");

app.Run();

