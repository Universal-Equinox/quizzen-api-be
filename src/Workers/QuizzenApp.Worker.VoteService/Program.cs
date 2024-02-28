using Microsoft.Extensions.Hosting;
using QuizzenApp.Worker.VoteService;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<Worker>();
});

var host = builder.Build();
await host.RunAsync();
