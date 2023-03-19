using Dooh.com;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public interface IRepository
    {
        Task<int> SaveAll(List<Ad> ads);

        Task<List<Ad>> GetAll();
    }


    public class Repository : IRepository
    {
        TContext _context;
        public Repository(TContext context)
        {
            _context = context;
        }

        public async Task<List<Ad>> GetAll()
        {
            return await _context.Ads.ToListAsync();
        }

        public async Task<int> SaveAll(List<Ad> ads)
        {
            await _context.AddRangeAsync(ads);
            return await _context.SaveChangesAsync();

        }
    }
}
