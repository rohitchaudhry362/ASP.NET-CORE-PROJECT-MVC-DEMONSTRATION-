using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RCPatients.Models;

namespace RCPatients.Controllers
{
    //contains action to perform CRUD operation on DespensingUnit table of Patients database
    //created by Rohit Chaudhry on 4th June,2020
    public class RCDispensingUnitController : Controller
    {
        private readonly PatientsContext _context;

        public RCDispensingUnitController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RCDispensingUnit
        //Lists all the DispensingUnit information
        public async Task<IActionResult> Index()
        {
            return View(await _context.DispensingUnit.ToListAsync());
        }

        // GET: RCDispensingUnit/Details/5
        //Gives details for the particular DispensingUnit
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit
                .FirstOrDefaultAsync(m => m.DispensingCode == id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }

            return View(dispensingUnit);
        }

        // GET: RCDispensingUnit/Create
        //returns a view that generates a form to create new DispensingUnit
        public IActionResult Create()
        {
            return View();
        }

        // POST: RCDispensingUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //saves the data entered by the user to the DispensingUnit table and returns to index page
        public async Task<IActionResult> Create([Bind("DispensingCode")] DispensingUnit dispensingUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispensingUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispensingUnit);
        }

        // GET: RCDispensingUnit/Edit/5
        //returns a view that generates the form to edit particular DispensingUnit information
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit.FindAsync(id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }
            return View(dispensingUnit);
        }

        // POST: RCDispensingUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //updates the data for particular DispensingUnit and returns to index page
        public async Task<IActionResult> Edit(string id, [Bind("DispensingCode")] DispensingUnit dispensingUnit)
        {
            if (id != dispensingUnit.DispensingCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispensingUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispensingUnitExists(dispensingUnit.DispensingCode))
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
            return View(dispensingUnit);
        }

        // GET: RCDispensingUnit/Delete/5
        //returns a view that generates the form to delete specific DispensingUnit
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit
                .FirstOrDefaultAsync(m => m.DispensingCode == id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }

            return View(dispensingUnit);
        }

        // POST: RCDispensingUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //deletes the particular DispensingUnit from database and returns to index page
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dispensingUnit = await _context.DispensingUnit.FindAsync(id);
            _context.DispensingUnit.Remove(dispensingUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //checks if the particular DispensingUnit exists or not
        private bool DispensingUnitExists(string id)
        {
            return _context.DispensingUnit.Any(e => e.DispensingCode == id);
        }
    }
}
