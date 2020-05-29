using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

class Response
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
            using (var rsp = await _http.GetAsync("/data"))
            {
                var str = await rsp.Content.ReadAsStringAsync();

                // deserialize
                var obj = JsonConvert.DeserializeObject<Response>(str);

                // serialize
                var json = JsonConvert.SerializeObject(obj);

                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(json);
            }
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.Map("/test", HandleTest);

        app.Run(async ctx =>
        {
            //await ctx.Response.WriteAsync($"Hello, {ctx.Request.Path}");
            await ctx.Response.WriteAsync("updated");
        });
    }
}
