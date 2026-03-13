using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThreadController:ControllerBase
    {
        private readonly AppDbContext _DbContext;
        public ThreadController(AppDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        [HttpGet]
        public async Task<IEnumerable<Thread>> Get()
        {
            var data = await _DbContext.Thread.AsNoTracking().ToListAsync();
            return data;
        }
    }
}