// See https://aka.ms/new-console-template for more information
using ConsoleApp2;
using Dooh.com;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


//Console.WriteLine("Hello, World!");

IConfiguration _config;
IHost host = Host.CreateDefaultBuilder().ConfigureAppConfiguration((hostContext, config) => {

    config.Sources.Clear();
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

}).ConfigureServices((hostContext,services) =>
{
    
    services.AddScoped<IService, Service>();
    services.AddDbContext<TContext>(opt =>
    {
     
    });
}).Build();

var env = host.Services.GetRequiredService<IHostEnvironment>();
var context = host.Services.GetRequiredService<TContext>();
_config = host.Services.GetRequiredService<IConfiguration>();
var service = host.Services.GetRequiredService<IService>();


var excelUpload= Path.Combine(env.ContentRootPath, _config["FilePath"].ToString());

service.Save(Helper.GetDataTabletFromCSVFile(excelUpload));


Console.WriteLine("Data successfully saved, type 'y' and enter to output back to excel and graph");

string input = Console.ReadLine();

if (input.ToLower() == "y")
{
    service.GetDataIntoExcel();
}
    
