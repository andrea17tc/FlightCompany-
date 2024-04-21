
using System.Collections.Generic;
using Utils.model;

namespace Server.repository
{
    public interface IRepository<ID, E> where E:Entity<ID>
    {
        E findOne(ID id);
        IEnumerable<E> findAll();
    
        void save(E entity);
    
        void delete(ID id);
   
        void update(E entity);
    
    }
}