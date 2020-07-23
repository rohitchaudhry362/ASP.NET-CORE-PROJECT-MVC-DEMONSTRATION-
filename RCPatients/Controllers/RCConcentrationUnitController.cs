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
    //contains action to perform CRUD operation on ConcentrationUnit table of Patients database
    //created by Rohit Chaudhry on 4th June,2020
    public class RCConcentrationUnitController : Controller
    {
        private readonly PatientsContext _context;

        public RCConcentrationUnitController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RCConcentrationUnit
        //Lists all the ConcentrationUnit information
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConcentrationUnit.ToListAsync());
        }

        // GET: RCConcentrationUnit/Details/5
        //Gives details for the particular ConcentrationUnit
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // GET: RCConcentrationUnit/Create
        //returns a view that generates a form to create new ConcentrationUnit
        public IActionResult Create()
        {
            return View();
        }

        // POST: RCConcentrationUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //saves the data entered by the user to the ConcentrationUnit table and returns to index page
        public async Task<IActionResult> Create([Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concentrationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concentrationUnit);
        }

        // GET: RCConcentrationUnit/Edit/5
        //returns a view that generates the form to edit particular ConcentrationUnit information
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }
            return View(concentrationUnit);
        }

        // POST: RCConcentrationUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //updates the data for particular ConcentrationUnit and returns to index page
        public async Task<IActionResult> Edit(string id, [Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (id != concentrationUnit.ConcentrationCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concentrationUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcentrationUnitExists(concentrationUnit.ConcentrationCode))
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
            return View(concentrationUnit);
        }

        // GET: RCConcentrationUnit/Delete/5
        //returns a view that generates the form to delete specific ConcentrationUnit
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // POST: RCConcentrationUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //deletes the particular ConcentrationUnit from database and returns to index page
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            _context.ConcentrationUnit.Remove(concentrationUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //checks if the particular ConcentrationUnit exists or not
        private bool ConcentrationUnitExists(string id)
        {
            return _context.ConcentrationUnit.Any(e => e.ConcentrationCode == id);
        }
    }
}
