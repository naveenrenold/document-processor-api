using DocumentProcessor.Endpoints;
using DocumentProcessor.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();
builder.AddSqlConnection(builder.Configuration.GetConnectionString("Database") ?? "");
builder.AddCors(builder.Configuration.GetSection("AppSettings:AllowedUrls").Get<string[]>() ?? []);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.AddFormEndpoints();
app.AddProcessEndpoints();
app.UseCors("CorsPolicy");

app.Run();

