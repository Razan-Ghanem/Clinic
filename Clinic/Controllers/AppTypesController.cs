using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic.Data;
using Clinic.Models;

namespace Clinic.Controllers
{
    public class AppTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppTypes.ToListAsync());
        }

        // GET: AppTypes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appType = await _context.AppTypes
                .FirstOrDefaultAsync(m => m.AppId == id);
            if (appType == null)
            {
                return NotFound();
            }

            return View(appType);
        }

        // GET: AppTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppId,AppointementType")] AppType appType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appType);
        }

        // GET: AppTypes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appType = await _context.AppTypes.FindAsync(id);
            if (appType == null)
            {
                return NotFound();
            }
            return View(appType);
        }

        // POST: AppTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AppId,AppointementType")] AppType appType)
        {
            if (id != appType.AppId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppTypeExists(appType.AppId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appType);
        }

        // GET: AppTypes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appType = await _context.AppTypes
                .FirstOrDefaultAsync(m => m.AppId == id);
            if (appType == null)
            {
                return NotFound();
            }

            return View(appType);
        }

        // POST: AppTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var appType = await _context.AppTypes.FindAsync(id);
            _context.AppTypes.Remove(appType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppTypeExists(long id)
        {
            return _context.AppTypes.Any(e => e.AppId == id);
        }
    }
}
