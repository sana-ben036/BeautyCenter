using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models.Repositories
{
   public interface ICenterRepository<TEntity>
    {
        IEnumerable<TEntity> GetList();
        TEntity Get(Guid Id);
        void Create(TEntity entity);
        TEntity Delete(Guid Id);
        TEntity Update(TEntity entityChanges);
    }
}
