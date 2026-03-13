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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Thread thread)
        {
            try
            {
                if (thread == null)
                {
                    return BadRequest("Thread data is required.");
                }

                _DbContext.Thread.Add(thread);
                await _DbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = thread.Id }, thread);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Thread thread)
        {
            try
            {
                if (id != thread.Id)
                {
                    return BadRequest("ID mismatch.");
                }

                var existingThread = await _DbContext.Thread.FindAsync(id);
                if (existingThread == null)
                {
                    return NotFound();
                }

                existingThread.Title = thread.Title;
                existingThread.Content = thread.Content;
                existingThread.Meter = thread.Meter;
                existingThread.ReadyStock = thread.ReadyStock;

                await _DbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var thread = await _DbContext.Thread.FindAsync(id);
                if (thread == null)
                {
                    return NotFound();
                }

                _DbContext.Thread.Remove(thread);
                await _DbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}