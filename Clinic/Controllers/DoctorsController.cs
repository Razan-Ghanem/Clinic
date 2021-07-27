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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Clinic.Country;
using Clinic.Migrations;

namespace Clinic.Controllers
{
    public class DoctorsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public  async Task<IActionResult> Index(string SearchString, string currentFilter, int? pageNo, string sortField, string currentSortField, string currentSortOrder)

        {
            List<Doctor> doctors =await _context.Doctors.Include(d => d.Specialization).ToListAsync();
            int pageSize = 3;

            if (!String.IsNullOrEmpty(SearchString))
            {
                pageNo = 1;
            }
            else
            {
                SearchString = currentFilter ;
            }
            ViewData["CurrentSort"] = sortField;
            ViewBag.CurrentFilter = SearchString;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var doc = this.SortDoctorData(doctors.Where(s => s.DoctorName.Contains(SearchString)).ToList(), sortField, currentSortField, currentSortOrder);
                return View (  PagingList<Doctor>.CreateAsync(doc.AsQueryable<Doctor>(), pageNo ?? 1, pageSize));
            }
            else
            {
                var doc = this.SortDoctorData(doctors, sortField, currentSortField, currentSortOrder).ToList();
                return View(PagingList<Doctor>.CreateAsync(doc.AsQueryable<Doctor>(), pageNo ?? 1, pageSize));
            }

        }
        public async Task <IEnumerable<CountryModel>> Countries()
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
   

      
        private List<Doctor> SortDoctorData(List<Doctor> doctors, string sortField, string currentSortField, string currentSortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "DoctorName";
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

            var propertyInfo = typeof(Doctor).GetProperty(ViewBag.SortField);
            if (ViewBag.SortOrder == "Asc")
            {
                doctors = doctors.OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
            }
            else
            {
                doctors = doctors.OrderByDescending(s => propertyInfo.GetValue(s, null)).ToList();
            }
            return doctors;
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public async Task <IActionResult> CreateAsync()
        {
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "SpecializationName");
            ViewData["country"] = new SelectList(await this.Countries(), "Name", "Name");

            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Address,Notes,PhoneNumber,Email,MonthlySalary,IBAN,SpecializationId,Country")] Doctor doctor, CountryModel country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "SpecializationName", doctor.SpecializationId);
            ViewData["Country"] = new SelectList(await this.Countries(),"Name","Name",country.Name);

            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "SpecializationName", doctor.SpecializationId);
            ViewData["Country"] = new SelectList(await this.Countries(), "Name", "Name");

            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,Address,Notes,PhoneNumber,Email,MonthlySalary,IBAN,SpecializationId,Country")] Doctor doctor,CountryModel country)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
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
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id", doctor.SpecializationId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(long id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
