var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("not-found", () => Results.NotFound(new { wtf = "?" }));

app.MapGet("nested-response", () => Results.Ok(new { @this = new { isNested = 2 } }));

app.Run();

public partial class Program { }
