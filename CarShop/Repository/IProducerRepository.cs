using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Repository
{
    public interface IProducerRepository
    {
        Task AddProducer(Producer producer);
        Task<IEnumerable<Producer>> GetProducers();
        Producer GetProducer(int id);
        Task DeleteProducer(int id);
        Task UpdateProducer(Producer producer);
    }
}
