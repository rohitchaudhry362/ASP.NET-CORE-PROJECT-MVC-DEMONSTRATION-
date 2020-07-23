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
    public class RCPatientTreatmentController : Controller
    {
        private readonly PatientsContext _context;

        public RCPatientTreatmentController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RCPatientTreatment
        public async Task<IActionResult> Index(int PatientDiagnosisId, string FName, string LName, string diagnosisName)
        {
            if (!string.IsNullOrEmpty(PatientDiagnosisId.ToString()))
            {
                Response.Cookies.Append("PatientDiagnosisId", PatientDiagnosisId.ToString());
            }
            else if (Request.Query["PatientDiagnosisId"].Any())
            {
                Response.Cookies.Append("PatientDiagnosisId", Request.Query["PatientDiagnosisId"].ToString());

                PatientDiagnosisId = Convert.ToInt32(Request.Query["PatientDiagnosisId"]);
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                PatientDiagnosisId = (int)HttpContext.Session.GetInt32("PatientDiagnosisId");
            }
            else
            {
                TempData["message"] = "Select any of the Patient Diagnosis first";
                return RedirectToAction("Index", "RCPatientDiagnosis");
            }
            var patientsContext = _context.PatientTreatment.Include(p => p.PatientDiagnosis).Include(t => t.Treatment)
                                    .Where(p => p.PatientDiagnosisId == PatientDiagnosisId)
                                    .OrderByDescending(p => p.DatePrescribed);
            ViewData["LName"] = LName;
            ViewData["FName"] = FName;
            ViewData["diagnosisName"] = diagnosisName;
            return View(await patientsContext.ToListAsync());
        }

        // GET: RCPatientTreatment/Details/5
        public async Task<IActionResult> Details(int? id, string diagnosisName)
        {
            string PatDiaId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatDiaId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                PatDiaId = HttpContext.Session.GetString("PatientDiagnosisId");
            }
            var PatDiagnosis = _context.PatientDiagnosis.Where(r => r.PatientDiagnosisId == Convert.ToInt32(PatDiaId)).FirstOrDefault();
            var Pat = _context.Patient.Where(r => r.PatientId == PatDiagnosis.PatientId).FirstOrDefault();
            ViewData["LName"] = Pat.LastName;
            ViewData["FName"] = Pat.FirstName;
            ViewData["diagnosisName"] = diagnosisName;
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // GET: RCPatientTreatment/Create
        public IActionResult Create(string diagnosisName)
        {
            string PatientDiagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                PatientDiagnosisId = HttpContext.Session.GetString("PatientDiagnosisId");
            }

            var PatDiagnosis = _context.PatientDiagnosis.Where(r => r.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            var Pat = _context.Patient.Where(r => r.PatientId == PatDiagnosis.PatientId).FirstOrDefault();

            ViewData["LName"] = Pat.LastName;
            ViewData["FName"] = Pat.FirstName;
            ViewData["diagnosisName"] = diagnosisName;

            var Diag = _context.Diagnosis.Where(x => x.Name == diagnosisName).FirstOrDefault();
            int diagId = Diag.DiagnosisId;

            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(x => x.DiagnosisId == diagId), "TreatmentId", "Name");
            return View();
        }

        // POST: RCPatientTreatment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            string PatientDiagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                PatientDiagnosisId = HttpContext.Session.GetString("PatientDiagnosisId");
            }
            patientTreatment.PatientDiagnosisId = Convert.ToInt32(PatientDiagnosisId);
            if (ModelState.IsValid)
            {
                _context.Add(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // GET: RCPatientTreatment/Edit/5
        public async Task<IActionResult> Edit(int? id, string diagnosisName)
        {
            string PatDiaId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatDiaId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                PatDiaId = HttpContext.Session.GetString("PatientDiagnosisId");
            }
            var PatDiagnosis = _context.PatientDiagnosis.Where(r => r.PatientDiagnosisId == Convert.ToInt32(PatDiaId)).FirstOrDefault();
            var Pat = _context.Patient.Where(r => r.PatientId == PatDiagnosis.PatientId).FirstOrDefault();
            ViewData["LName"] = Pat.LastName;
            ViewData["FName"] = Pat.FirstName;
            ViewData["diagnosisName"] = diagnosisName;
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment.FindAsync(id);
            ViewData["Date"] = patientTreatment.DatePrescribed.ToString("dd MMMM yyyy hh:mm");
            if (patientTreatment == null)
            {
                return NotFound();
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // POST: RCPatientTreatment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            if (id != patientTreatment.PatientTreatmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTreatmentExists(patientTreatment.PatientTreatmentId))
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
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // GET: RCPatientTreatment/Delete/5
        public async Task<IActionResult> Delete(int? id, String diagnosisName)
        {
            string PatDiaId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatDiaId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else if (HttpContext.Session.GetString("PatientDiagnosisId") != null)
            {
                PatDiaId = HttpContext.Session.GetString("PatientDiagnosisId");
            }
            var PatDiagnosis = _context.PatientDiagnosis.Where(r => r.PatientDiagnosisId == Convert.ToInt32(PatDiaId)).FirstOrDefault();
            var Pat = _context.Patient.Where(r => r.PatientId == PatDiagnosis.PatientId).FirstOrDefault();
            ViewData["LName"] = Pat.LastName;
            ViewData["FName"] = Pat.FirstName;
            ViewData["diagnosisName"] = diagnosisName;
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // POST: RCPatientTreatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientTreatment = await _context.PatientTreatment.FindAsync(id);
            _context.PatientTreatment.Remove(patientTreatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientTreatmentExists(int id)
        {
            return _context.PatientTreatment.Any(e => e.PatientTreatmentId == id);
        }
    }
}
