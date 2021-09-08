using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using Microsoft.EntityFrameworkCore;
namespace CarShop.Repository
{
    public class ProducerRepository: IProducerRepository
    {
        DbCarShopContext _context;
        public ProducerRepository(DbCarShopContext context)
        {
            _context = context;
        }

        public async void AddProducer(Producer producer)
        {
            try
            {
                _context.Add(producer);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Producer GetProducer(int id)
        {
            Producer producer = _context.Producers.Where(a => a.ProducerId == id).FirstOrDefault();
            return  producer;
        }

        public async Task<IEnumerable<Producer>> GetProducers()
        {
            return await _context.Producers.ToListAsync();
        }

        public async void UpdateProducer(Producer producer)
        {
            try
            {
                _context.Update(producer);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }

        public async void DeleteProducer(int id)
        {
            Producer producer = await _context.Producers.FirstOrDefaultAsync(a => a.ProducerId == id);
            try
            {
                _context.Remove(producer);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
