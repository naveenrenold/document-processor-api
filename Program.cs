using GitReminder.Endpoints;
using GitReminder.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.AddPullRequestEndpoints();

app.Run();

