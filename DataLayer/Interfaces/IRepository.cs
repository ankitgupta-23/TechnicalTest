using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IRepository
    {
        public Task<bool> CreateEntity(Entity entity);
        public Task<bool> UpdateEntity(Entity entity);
        public Task<bool> DeleteEntity(Entity entity);

        public Task<bool> SaveChangesWithRetry();

        public Task<IEnumerable<Entity>> GetEntities();
        
        public Task<IEnumerable<Entity>> GetFilteredEntities(string? searchQuery, string? gender, DateTime? startDate, DateTime? endDate, List<string>? countries, string? orderBy, int pageNo=1, int pageSize=5);
        public Task<Entity> GetEntityById(string id);
    }
}
