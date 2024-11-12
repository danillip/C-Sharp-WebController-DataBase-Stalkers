using System;
using System.Threading.Tasks;
using DataBaze.Stalker.Models;
using DataBaze.Stalker.Data;

namespace DataBaze.Services
{
    public class ChangeLogService
    {
        private readonly StalkerContext _context;

        public ChangeLogService(StalkerContext context)
        {
            _context = context;
        }

        public async Task LogChangeAsync(ChangeLog changeLog)
        {
            _context.ChangeLogs.Add(changeLog);
            await _context.SaveChangesAsync();
        }
    }
}
