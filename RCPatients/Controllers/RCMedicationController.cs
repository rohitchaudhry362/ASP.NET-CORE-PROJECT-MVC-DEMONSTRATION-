using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RCPatients.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RCPatients.Controllers
{
    public class RCMedicationController : Controller
    {
        private readonly PatientsContext _context;

        public RCMedicationController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RCMedication
        public async Task<IActionResult> Index(string MTypeId)
        {
            if (!string.IsNullOrEmpty(MTypeId))
            {
                Response.Cookies.Append("MTypeId", MTypeId);
                HttpContext.Session.SetString("MTypeId", MTypeId);
            }
            else if (Request.Query["MTypeId"].Any())
            {
                Response.Cookies.Append("MTypeId", Request.Query["MTypeId"].ToString());
                HttpContext.Session.SetString("MTypeId", Request.Query["MTypeId"].ToString());
                MTypeId = Request.Query["MTypeId"].ToString();
            }
            else if(Request.Cookies["MtypeId"] != null)
            {
                MTypeId = Request.Cookies["MtypeId"].ToString();
            }
            else if(HttpContext.Session.GetString("MtypeId") != null)
            {
                MTypeId = HttpContext.Session.GetString("MtypeId");
            }
            else
            {
                TempData["message"] = "Please select any medication type";
                return RedirectToAction("Index", "MedicationType");
            }

            var MedicationType = _context.MedicationType.Where(r => r.MedicationTypeId == Int32.Parse(MTypeId)).FirstOrDefault();
            ViewData["MTypeId"] = MTypeId;
            ViewData["MedicationTypeName"] = MedicationType.Name;

            var medicationContext = _context.Medication.Include(r => r.MedicationType).Include(r => r.ConcentrationCodeNavigation)
                                    .Include(r => r.DispensingCodeNavigation)
                .Where(r => r.MedicationTypeId == Int32.Parse(MTypeId))
                .OrderBy(r=> r.Name)
                .ThenBy(r => r.Concentration);
            return View(await medicationContext.ToListAsync());
        }

        // GET: RCMedication/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }
            ViewData["medication"] = medication.Name;
            return View(medication);
        }

        // GET: RCMedication/Create
        public IActionResult Create()
        {
            string MedicationTypeCode = string.Empty;
            if(Request.Cookies["MTypeId"] != null)
            {
                MedicationTypeCode = Request.Cookies["MTypeId"].ToString();
            }
            else if(HttpContext.Session.GetString("MTypeId") != null)
            {
                MedicationTypeCode = HttpContext.Session.GetString("MTypeId");
            }
            var MedType = _context.MedicationType.Where(r => r.MedicationTypeId == Int32.Parse(MedicationTypeCode)).FirstOrDefault();
            ViewData["MedicationTypeCode"] = MedicationTypeCode;
            ViewData["MedicationTypeName"] = MedType.Name;

            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(r=>r.ConcentrationCode), "ConcentrationCode", "ConcentrationCode");
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(r=>r.DispensingCode), "DispensingCode", "DispensingCode");
            return View();
        }

        // POST: RCMedication/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            string MedicationTypeCode = string.Empty;
            if (Request.Cookies["MTypeId"] != null)
            {
                MedicationTypeCode = Request.Cookies["MTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MTypeId") != null)
            {
                MedicationTypeCode = HttpContext.Session.GetString("MTypeId");
            }

            var isDuplicate = _context.Medication.Where(r => r.Name == medication.Name && r.Concentration == medication.Concentration
            && r.ConcentrationCode == medication.ConcentrationCode);
            if (isDuplicate.Any())
            {
                ModelState.AddModelError("", "There is already a pair with name: "+medication.Name+ " ,concentration: "
                    +medication.Concentration+" and concentrationCode: "+medication.ConcentrationCode);
            }

            if (ModelState.IsValid)
            {
                medication.MedicationTypeId = Int32.Parse(MedicationTypeCode);
                _context.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            return View(medication);
        }

        // GET: RCMedication/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication.FindAsync(id);
            ViewData["medication"] = medication.Name;
            if (medication == null)
            {
                return NotFound();
            }

            ViewData["ConcentrationCode"] = medication.ConcentrationCode;
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            return View(medication);
        }

        // POST: RCMedication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            if (id != medication.Din)
            {
                return NotFound();
            }

            string MedicationTypeCode = string.Empty;
            if (Request.Cookies["MTypeId"] != null)
            {
                MedicationTypeCode = Request.Cookies["MTypeId"].ToString();
            }
            else if (HttpContext.Session.GetString("MTypeId") != null)
            {
                MedicationTypeCode = HttpContext.Session.GetString("MTypeId");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    medication.MedicationTypeId = Int32.Parse(MedicationTypeCode);
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.Din))
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
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            //ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: RCMedication/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }
            ViewData["medication"] = medication.Name;
            return View(medication);
        }

        // POST: RCMedication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var medication = await _context.Medication.FindAsync(id);
            _context.Medication.Remove(medication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationExists(string id)
        {
            return _context.Medication.Any(e => e.Din == id);
        }
    }
}
