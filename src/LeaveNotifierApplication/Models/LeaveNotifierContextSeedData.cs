using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveNotifierApplication.Models
{
    public class LeaveNotifierContextSeedData
    {
        private LeaveNotifierContext _context;

        public LeaveNotifierContextSeedData(LeaveNotifierContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            if (!_context.Leaves.Any())
            {
                var firstLeave = new Leave()
                {
                    dateCreated = DateTime.UtcNow,
                    justification = "first sample leave",
                    means = Means.EMAIL,
                    status = Status.UNFILED,
                    user = "" // todo
                };

                _context.Leaves.Add(firstLeave);

                await _context.SaveChangesAsync();
            }
        }
    }
}
