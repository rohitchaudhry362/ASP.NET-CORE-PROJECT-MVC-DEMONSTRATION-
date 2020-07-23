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
    //contains action to perform CRUD operation on Country table of Patients database
    //created by Rohit Chaudhry on 4th June,2020,2020
    public class RCCountryController : Controller
    {
        private readonly PatientsContext _context;

        public RCCountryController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RCCountry
        //Lists all the country information
        public async Task<IActionResult> Index()
        {
            return View(await _context.Country.ToListAsync());
        }

        // GET: RCCountry/Details/5
        //Gives details for the particular country
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: RCCountry/Create
        //returns a view that generates a form to create new country
        public IActionResult Create()
        {
            return View();
        }

        // POST: RCCountry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //saves the data entered by the user to the country table and returns to index page
        public async Task<IActionResult> Create([Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: RCCountry/Edit/5
        //returns a view that generates the form to edit particular country information
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: RCCountry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //updates the data for particular country and returns to index page
        public async Task<IActionResult> Edit(string id, [Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (id != country.CountryCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryCode))
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
            return View(country);
        }

        // GET: RCCountry/Delete/5
        //returns a view that generates the form to delete specific country
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: RCCountry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //deletes the particular country from database and returns to index page
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //checks if the particular country exists or not
        private bool CountryExists(string id)
        {
            return _context.Country.Any(e => e.CountryCode == id);
        }
    }
}
