using Dooh.com;
using Microsoft.EntityFrameworkCore;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Annotations;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp2
{
    public class Service : IService
    {
        private TContext _context;
        private string rootDirectory;
        private IConfiguration _Configuration;
        public Service(TContext context, IConfiguration configuration)
        {
            _context = context;
            _Configuration = configuration;
        }
        public async Task GetDataIntoExcel()
        {
            var ads= await _context.Ads.Take(100).ToListAsync();

            rootDirectory = Helper.GetRootDirectory();

            string excelOutputPath = Path.Combine(rootDirectory, _Configuration["excelOutputPath"]);

            string workSheetName = _Configuration["workSheetName"];

            string graphPath= Path.Combine(rootDirectory, _Configuration["graphPath"]);


            Helper.ExportToExcel(ads, excelOutputPath, workSheetName);

            Helper.CreateGraph(ads, graphPath);

           










        }

        public async Task<int> Save(List<Ad> Ads)
        {
            try
            {
                await _context.Ads.AddRangeAsync(Ads);
                return await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }

            return 1;
             
        }

        private class Chart
        {
            public Chart()
            {
            }
        }
    }

    public interface IService
    {
        Task<int> Save(List<Ad> Ads);

        Task GetDataIntoExcel();
    }
}
