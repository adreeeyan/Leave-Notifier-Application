using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveNotifierApplication.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveNotifierApplication.Data
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
                .Include(l => l.User)
                .ToList();
        }

        public IEnumerable<Leave> GetLeavesByUserName(string userName)
        {
            return _context.Leaves
                .Where(l => l.User.UserName == userName)
                .ToList();
        }

        public Leave GetLeaveById(int id)
        {
            return _context.Leaves
                .Include(l => l.User)
                .First(l => l.Id == id);
        }

        public IEnumerable<LeaveNotifierUser> GetAllUsers()
        {
            return _context.Users.Cast<LeaveNotifierUser>().ToList();
        }

        public LeaveNotifierUser GetUserByUserName(string userName)
        {
            return _context.Users.Cast<LeaveNotifierUser>().First(user => user.UserName == userName);
        }

        public async Task<bool> SaveAllAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
