using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Data;
using Clinic.Models;

namespace Clinic.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AppTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppType>>> GetAppTypes()
        {
            return await _context.AppTypes.ToListAsync();
        }

        // GET: api/AppTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppType>> GetAppType(long id)
        {
            var appType = await _context.AppTypes.FindAsync(id);

            if (appType == null)
            {
                return NotFound();
            }

            return appType;
        }

        // PUT: api/AppTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppType(long id, AppType appType)
        {
            if (id != appType.AppId)
            {
                return BadRequest();
            }

            _context.Entry(appType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AppTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppType>> PostAppType(AppType appType)
        {
            _context.AppTypes.Add(appType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppType", new { id = appType.AppId }, appType);
        }

        // DELETE: api/AppTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppType(long id)
        {
            var appType = await _context.AppTypes.FindAsync(id);
            if (appType == null)
            {
                return NotFound();
            }

            _context.AppTypes.Remove(appType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppTypeExists(long id)
        {
            return _context.AppTypes.Any(e => e.AppId == id);
        }
    }
}
