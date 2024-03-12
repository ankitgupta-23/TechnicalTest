
using DataLayer.AppDbContext;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace DataLayer.Implementations
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly ILogger<Repository> _logger;
        public Repository(ApplicationDbContext dbcontext, ILogger<Repository> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        public async Task<bool> CreateEntity(Entity entity)
        {

            _dbcontext.Entities.Add(entity);
            
            var res = await SaveChangesWithRetry();
            return res;
        }



        public async Task<bool> UpdateEntity(Entity entity)
        {
            bool ifExists = _dbcontext.Entities.Any(e => e.Id == entity.Id);

            if (!ifExists)
                return false;

            _dbcontext.Entities.Update(entity);
            
            var res = await SaveChangesWithRetry();
            return res;
        }


        public async Task<bool> DeleteEntity(Entity entity)
        {
            _dbcontext.Entities.Remove(entity);
            var res = await SaveChangesWithRetry();
            return res;
        }

        public async Task<IEnumerable<Entity>> GetEntities()
        {
            return await _dbcontext.Entities
                        .Include(e => e.Addresses)
                        .Include(e => e.Names)
                        .Include(e => e.Dates)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Entity>> GetFilteredEntities(string? searchQuery, string? gender, DateTime? startDate, DateTime? endDate, List<string>? countries, string orderBy, int pageNo = 1, int pageSize = 5)
        {

            var query = from e in _dbcontext.Entities select e;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(e =>
                    e.Addresses.Any(a => a.Country.Contains(searchQuery) || a.AddressLine.Contains(searchQuery)) ||
                    e.Names.Any(n => n.FirstName.Contains(searchQuery) || n.MiddleName.Contains(searchQuery) || n.Surname.Contains(searchQuery))
                );
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(e => e.Gender == gender);
            }

            if (startDate != null && endDate != null)
            {
                query = query.Where(e => e.Dates.Any(d => d._Date >= startDate && d._Date <= endDate));
            }

            if (countries != null && countries.Any())
            {
                query = query.Where(e => e.Addresses.Any(a => countries.Contains(a.Country)));
            }


            //Incase pageize is not reasonable, set it to default


            if (orderBy != null)
            {
                switch (orderBy.ToLower())
                {
                    case "name":
                        query = query.OrderBy(e => e.Names.FirstOrDefault().FirstName);
                        break;
                    case "date":
                        query = query.OrderBy(e => e.Dates.FirstOrDefault()._Date);
                        break;
                    case "country":
                        query = query.OrderBy(e => e.Addresses.FirstOrDefault().Country);
                        break;
                }
            }


            pageSize = pageSize > 20 ? 5 : pageSize;

            if (pageNo > 0)
            {
                query = query.Skip((pageNo - 1) * pageSize).Take(pageSize);
            }


            var data = query.Include(e => e.Addresses)
                        .Include(e => e.Names)
                        .Include(e => e.Dates);

            return data;
        }

        public async Task<Entity> GetEntityById(string id)
        {
            var res = await _dbcontext.Entities.Where(e => e.Id == id)
                            .Include(e => e.Addresses)
                            .Include(e => e.Names)
                            .Include(e => e.Dates)
                            .FirstOrDefaultAsync();

            return res;
        }


        public async Task<bool> SaveChangesWithRetry()
        {
            int max_retry = 3;
            int current_attempt = 1;

            TimeSpan delay = TimeSpan.FromSeconds(1.5);

            while (current_attempt <= max_retry)
            {
                try
                {
                    await _dbcontext.SaveChangesAsync();
                    _logger.LogInformation($"Database Write Successfull at attempt = {current_attempt}");

                    return true;
                   
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during database write operation. Retrying...");


                    _logger.LogInformation($"Retry attempt {current_attempt} in {delay} seconds.");


                    Task.Delay(delay).Wait();


                    delay *= 1.2;

                    current_attempt++;
                }
            }

            return false;
        }


    }
}