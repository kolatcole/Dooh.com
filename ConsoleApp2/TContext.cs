using Dooh.com;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp2
{
    public class TContext:DbContext
    {
        IConfiguration _config;
        public TContext(DbContextOptions<TContext> options, IConfiguration config):base(options)
        {
            _config = config;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-5SRAAK8\\SQLEXPRESS;Initial Catalog=CityWireDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            optionsBuilder.UseMySQL(_config.GetConnectionString("defaultConnection"));
            
        }

        



        public DbSet<Ad> Ads { get; set; }
    }
}
