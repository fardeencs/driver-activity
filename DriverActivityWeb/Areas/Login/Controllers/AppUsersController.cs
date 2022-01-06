#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DriverActivityWeb.Data;
using DriverActivityWeb.Models;

namespace DriverActivityWeb.Areas.Login.Controllers
{
    [Area("Login")]
    public class AppUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login/AppUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppUser.ToListAsync());
        }

        // GET: Login/AppUsers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUser
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // GET: Login/AppUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Login/AppUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FileID,NameEn,UserName,Password,StaffId,QID,VehicleID,IsActive,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: Login/AppUsers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUser.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: Login/AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,FileID,NameEn,UserName,Password,StaffId,QID,VehicleID,IsActive,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] AppUser appUser)
        {
            if (id != appUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.UserId))
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
            return View(appUser);
        }

        // GET: Login/AppUsers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUser
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: Login/AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var appUser = await _context.AppUser.FindAsync(id);
            _context.AppUser.Remove(appUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(long id)
        {
            return _context.AppUser.Any(e => e.UserId == id);
        }
    }
}
