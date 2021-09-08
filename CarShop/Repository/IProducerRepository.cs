using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Repository
{
    public interface IProducerRepository
    {
        void AddProducer(Producer producer);
        Task<IEnumerable<Producer>> GetProducers();
        Producer GetProducer(int id);
        void DeleteProducer(int id);
        void UpdateProducer(Producer producer);
    }
}
