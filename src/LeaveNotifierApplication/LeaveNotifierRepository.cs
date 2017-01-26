using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveNotifierApplication.Models;

namespace LeaveNotifierApplication
{
    public class LeaveNotifierRepository : ILeaveNotifierRepository
    {
        private LeaveNotifierDbContext _context;

        public LeaveNotifierRepository(LeaveNotifierDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public IEnumerable<Leave> GetAllLeaves()
        {
            return _context.Leaves
                .OrderByDescending(l => l.DateCreated)
                .ToList();
        }

        public async Task<bool> SaveAllAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
