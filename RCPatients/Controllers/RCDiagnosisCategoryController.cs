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
    //contains action to perform CRUD operation on DiagnosisCategory table of Patients database
    //created by Rohit Chaudhry on 4th June,2020
    public class RCDiagnosisCategoryController : Controller
    {
        private readonly PatientsContext _context;

        public RCDiagnosisCategoryController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RCDiagnosisCategory
        //Lists all the DiagnosisCategory information
        public async Task<IActionResult> Index()
        {
            return View(await _context.DiagnosisCategory.ToListAsync());
        }

        // GET: RCDiagnosisCategory/Details/5
        //Gives details for the particular DiagnosisCategory
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }

            return View(diagnosisCategory);
        }

        // GET: RCDiagnosisCategory/Create
        //returns a view that generates a form to create new DiagnosisCategory
        public IActionResult Create()
        {
            return View();
        }

        // POST: RCDiagnosisCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //saves the data entered by the user to the DiagnosisCategory table and returns to index page
        public async Task<IActionResult> Create([Bind("Id,Name")] DiagnosisCategory diagnosisCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosisCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diagnosisCategory);
        }

        // GET: RCDiagnosisCategory/Edit/5
        //returns a view that generates the form to edit particular DiagnosisCategory information
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory.FindAsync(id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }
            return View(diagnosisCategory);
        }

        // POST: RCDiagnosisCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //updates the data for particular DiagnosisCategory and returns to index page
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] DiagnosisCategory diagnosisCategory)
        {
            if (id != diagnosisCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosisCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisCategoryExists(diagnosisCategory.Id))
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
            return View(diagnosisCategory);
        }

        // GET: RCDiagnosisCategory/Delete/5
        //returns a view that generates the form to delete specific DiagnosisCategory
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }

            return View(diagnosisCategory);
        }

        // POST: RCDiagnosisCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //deletes the particular DiagnosisCategory from database and returns to index page
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnosisCategory = await _context.DiagnosisCategory.FindAsync(id);
            _context.DiagnosisCategory.Remove(diagnosisCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //checks if the particular DiagnosisCategory exists or not
        private bool DiagnosisCategoryExists(int id)
        {
            return _context.DiagnosisCategory.Any(e => e.Id == id);
        }
    }
}
