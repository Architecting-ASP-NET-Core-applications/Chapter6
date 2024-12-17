using Chapter6CustomMiddleware.Middleware;
using Chapter6CustomMiddleware.Models;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddTransient<IMyInterface>(provider =>
//{
//    switch (myConfiguration.ImplementationType)
//    {
//        case 0:
//            return new ImplementationA();
//        case 1:
//            return new ImplementationA();
//        case 2:
//            return new ImplementationA();
//        default:
//            throw NotSupportedException();
//    }
//});

//var myServiceConfig = Configuration.GetSection
//    ("MyServiceConfig").Get<MyServiceConfig>();
//services.AddSingleton(new MyService(myServiceConfig));




///
/// Examples for page 21
///
builder.Services.AddSingleton<IMyInterface, ImplementationA>();
builder.Services.AddSingleton<IMyInterface, ImplementationB>();
builder.Services.AddSingleton<IMyInterface, ImplementationC>();



///
/// Examples for page 22: Options pattern
///
builder.Services.Configure<MyServiceConfig>
        (options => builder.Configuration.GetSection("MyServiceConfig")
                   .Bind(options));



var app = builder.Build();

///
/// Examples for page 21
///
var implementation = app.Services.GetService<IMyInterface>();



///
/// Examples for page 22
///
var implementations = app.Services.GetServices<IMyInterface>();
var implementationA = implementations.OfType<ImplementationA>().FirstOrDefault();




//
// Example page 13
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else if (app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/ErrorStaging");
    app.UseHsts();
}
else if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();

}


app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();


///
/// Example for page 16
///
app.UseMiddleware<RequestLoggingMiddleware>();


app.UseMiddleware<StaticFileTerminalMiddleware>();





//
// Example page 8
//
//app.Use(async (context, next) =>
//{
//    // Here we can write on the Response.
//    await next.Invoke();
//    // Here we have not to write to the Response.
//});

//
// Example page 7
//
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Welcome to Chapter6!");
//});


///
/// Example for page 16
///
app.UseRequestLogging();

//
// Example page 9
//
app.MapGet("/short-circuit", () => "Short circuiting!").ShortCircuit();
app.MapShortCircuit(404);

app.Run();






internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}



/// <summary>
/// BONUS: Example for minimal terminal middleware for static files.
/// </summary>
/// <param name="next"></param>
/// <param name="logger"></param>
public class StaticFileTerminalMiddleware(RequestDelegate next, ILogger<StaticFileTerminalMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (IsStaticFileRequest(context.Request))
        {
            // Process the request here (serve the static file)
            await ServeStaticFileAsync(context);
            // Log the action
            logger.LogInformation("Served a static file and terminated the request pipeline.");
            // Terminate the pipeline
            return;
        }
        // Not a static file request, continue with the next middleware
        await next(context);
    }

    private bool IsStaticFileRequest(HttpRequest request)
    {
        // Implement logic to determine if the request is for a static file
        // For example:
        return request.Path.ToString().EndsWith(".jpg") || request.Path.ToString().EndsWith(".html");
    }

    private async Task ServeStaticFileAsync(HttpContext context)
    {
        // Logic to serve the file
        // This is a simplified example using static file path mapping
        var filePath = $"wwwroot{context.Request.Path}";
        if (File.Exists(filePath))
        {
            context.Response.ContentType = "application/octet-stream";
            await context.Response.SendFileAsync(filePath);
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }
}

