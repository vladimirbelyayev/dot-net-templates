using WebApiTemplate.Bootstrap;

var builder = WebApplication.CreateBuilder(args)
    .AddSerilogLogging()
    .AddApplicationInsights()
    .AddWebApiFeatures()
    .AddAuth()
    .AddCorsUrls()
    .AddDependencies()
    .AddRequestLocalization()
    .AddSwaggerServices();

var app = builder.Build()
    .UseWebApiFeatures()
    .UseCORS()
    .UseAuth()
    .UseRequestLocalization()
    .UseSwaggerPage();

app.Run();
