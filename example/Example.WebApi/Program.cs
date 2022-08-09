var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("not-found", () => Results.NotFound(new { wtf = "?" }));

app.Run();

public partial class Program { }
