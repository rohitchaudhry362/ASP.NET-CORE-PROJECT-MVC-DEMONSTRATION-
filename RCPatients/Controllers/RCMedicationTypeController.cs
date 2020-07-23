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
    //contains action to perform CRUD operation on MedicationType table of Patients database
    //created by Rohit Chaudhry on 4th June,2020
    public class RCMedicationTypeController : Controller
    {
        private readonly PatientsContext _context;

        public RCMedicationTypeController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RCMedicationType
        //Lists all the Medication type information
        public async Task<IActionResult> Index()
        { 
            return View(await _context.MedicationType.OrderBy(m => m.Name).ToListAsync());
        }

        // GET: RCMedicationType/Details/5
        //Gives details for the particular Medication type
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }
            return View(medicationType);
        }

        // GET: RCMedicationType/Create
        //returns a view that generates a form to create new Medication type
        public IActionResult Create()
        {
            return View();
        }

        // POST: RCMedicationType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //saves the data entered by the user to the country table and returns to index page
        public async Task<IActionResult> Create([Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicationType);
        }

        // GET: RCMedicationType/Edit/5
        //returns a view that generates the form to edit particular Medication type information
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType.FindAsync(id);
            if (medicationType == null)
            {
                return NotFound();
            }
            return View(medicationType);
        }

        // POST: RCMedicationType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //updates the data for particular Medication type and returns to index page
        public async Task<IActionResult> Edit(int id, [Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (id != medicationType.MedicationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationTypeExists(medicationType.MedicationTypeId))
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
            return View(medicationType);
        }

        // GET: RCMedicationType/Delete/5
        //returns a view that generates the form to delete specific Medication type
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // POST: RCMedicationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //deletes the particular Medication type from database and returns to index page
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationType = await _context.MedicationType.FindAsync(id);
            _context.MedicationType.Remove(medicationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //checks if the particular Medication type exists or not
        private bool MedicationTypeExists(int id)
        {
            return _context.MedicationType.Any(e => e.MedicationTypeId == id);
        }
    }
}
