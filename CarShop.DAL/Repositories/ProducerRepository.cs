using CarShop.DAL.Data;
using CarShop.DOM.Database;
using CarShop.DOM.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.DAL.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private readonly DbCarShopContext _context;

        public ProducerRepository(DbCarShopContext context)
        {
            _context = context;
        }
        public void Add(Producer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Producers.Add(entity);
        }

        public void Delete(int id)
        {
            Producer producer = _context.Producers.FirstOrDefault(p => p.ProducerId == id);
            if (producer == null)
            {
                throw new KeyNotFoundException(nameof(producer));
            }
            _context.Producers.Remove(producer);
        }

        public bool Exists(int id)
        {
            return _context.Producers.Any(e => e.ProducerId == id);
        }

        public IEnumerable<Producer> GetAll()
        {
            return _context.Producers.ToList();
        }

        public Producer GetById(int id)
        {
            return _context.Producers.FirstOrDefault(p => p.ProducerId == id);
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Update(Producer entity)
        {
            Producer producer = _context.Producers.FirstOrDefault(p => p.ProducerId == entity.ProducerId);
            if (producer == null)
            {
                throw new KeyNotFoundException(nameof(entity));
            }
            try
            {
                _context.Producers.Update(entity);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(dbUpdateConcurrencyException.Message, dbUpdateConcurrencyException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DbUpdateException(dbUpdateException.Message, dbUpdateException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
