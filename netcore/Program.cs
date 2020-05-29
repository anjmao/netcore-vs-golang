using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Utf8Json;

class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
        var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();
 
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseConfiguration(config)
            .UseUrls("http://*:5000")
            .UseStartup<Startup>();

        return host;
    }
}

public struct Response
{
    public string Id { get; set; }
    public string Name { get; set; }
    public long Time { get; set; }
}

class Startup
{
    private static readonly HttpMessageHandler _httpHandler = new HttpClientHandler
    {
        MaxConnectionsPerServer = 4000
    };

    private static readonly HttpClient _http = new HttpClient(_httpHandler)
    {
        BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("HOST")}:5002")
    };

    private static void HandleTest(IApplicationBuilder app)
    {
        app.Run(async ctx =>
        {
            await using var rsp = await _http.GetStreamAsync("/data");

            // deserialize
            var obj = await JsonSerializer.DeserializeAsync<Response>(rsp);

            // serialize
            ctx.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(ctx.Response.Body, obj);
        });
    }
    public void Configure(IApplicationBuilder app)
    {
        app.Map("/test", HandleTest);

        app.Run(async ctx =>
        {
            await ctx.Response.WriteAsync($"Hello, {ctx.Request.Path}");
        });
    }
}
