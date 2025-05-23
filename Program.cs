using DocumentProcessor.Endpoints;
using DocumentProcessor.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();
builder.AddSqlConnection(builder.Configuration.GetConnectionString("Database") ?? "");
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.AddEndpoints();

app.Run();

