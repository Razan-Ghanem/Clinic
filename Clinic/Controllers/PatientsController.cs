using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic.Data;
using Clinic.Models;
using Clinic.Paging;
using Clinic.Country;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Clinic.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index(string SearchString, string currentFilter, int? pageNo, string sortField, string currentSortField, string currentSortOrder)
        {
            List<Patient> patients =await _context.Patients.ToListAsync();
            int pageSize = 3;

            if (!String.IsNullOrEmpty(SearchString))
            {
                pageNo = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewData["CurrentSort"] = sortField;
            ViewBag.CurrentFilter = SearchString;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var pa = this.SortPatientData(patients.Where(s => s.PatientName.Contains(SearchString)).ToList(), sortField, currentSortField, currentSortOrder);
                return View(PagingList<Patient>.CreateAsync(pa.AsQueryable<Patient>(), pageNo ?? 1, pageSize));
            } 
            else
            {
                var doc = this.SortPatientData(patients, sortField, currentSortField, currentSortOrder).ToList();
                return View(PagingList<Patient>.CreateAsync(doc.AsQueryable<Patient>(), pageNo ?? 1, pageSize));
            }

        }

        private List<Patient> SortPatientData(List<Patient> patients, string sortField, string currentSortField, string currentSortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "PatientName";
                ViewBag.SortOrder = "Asc";
            }
            else
            {
                if (currentSortField == sortField)
                {
                    ViewBag.SortOrder = currentSortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    ViewBag.SortOrder = "Asc";
                }
                ViewBag.SortField = sortField;
            }

            var propertyInfo = typeof(Patient).GetProperty(ViewBag.SortField);
            if (ViewBag.SortOrder == "Asc")
            {
                patients = patients.OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
            }
            else
            {
                patients = patients.OrderByDescending(s => propertyInfo.GetValue(s, null)).ToList();
            }
            return patients;
        }
    
        public async Task<IEnumerable<CountryModel>> Countries()
        {
        string Baseurl = "https://restcountries.eu/rest/v2/all";
        List<CountryModel> country = new List<CountryModel>();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Baseurl);

            client.DefaultRequestHeaders.Clear();
            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Res = await client.GetAsync(Baseurl);

            if (Res.IsSuccessStatusCode)
            {
                var CountryResponse = Res.Content.ReadAsStringAsync().Result;
                country = JsonConvert.DeserializeObject<List<CountryModel>>(CountryResponse);

            }
            return (country);

        }
    }


    // GET: Patients/Details/5
    public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["country"] = new SelectList(await this.Countries(), "Name", "Name");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDay,Gender,PhoneNumber,Email,Address,RegistrationDate,SSN,Country")] Patient patient,CountryModel country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                ViewData["Country"] = new SelectList(await this.Countries(),"Name","Name",country.Name);
                return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name");
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,BirthDay,Gender,PhoneNumber,Email,Address,RegistrationDate,SSN, Country")] Patient patient,CountryModel country)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name",country.Name);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(long id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
