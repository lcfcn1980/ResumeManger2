using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResumeManger2.Data;
using ResumeManger2.Models;

namespace ResumeManger2.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ResumeDbContext _context;

        private readonly IWebHostEnvironment _webHost;

        public ResumeController(ResumeDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public IActionResult Index()
        {
            List<Applicant> applicants;
            applicants = _context.Applicants.ToList();
            return View(applicants);
        }

        public IActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.Experiences.Add(new Experience() { ExperienceId = 1 });
            ViewBag.Gender = GetGender();
            return View(applicant);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Applicant applicant = _context.Applicants.Include(e => e.Experiences).Where(a => a.Id == id).FirstOrDefault();
            ViewBag.Gender = GetGender();
            return View(applicant);
        }

        [HttpPost]
        public IActionResult Edit(Applicant applicant)
        {
            List<Experience> expDetails = _context.Experiences.Where(d => d.ApplicantId == applicant.Id).ToList();
            _context.Experiences.RemoveRange(expDetails);
            _context.SaveChanges();

            applicant.Experiences.RemoveAll(n => n.YearsWorked == 0);
            applicant.Experiences.RemoveAll(n => n.IsDeleted == true);

            if (applicant.ProfilePhoto !=null)
            {
                string uniqueFileName = GetUploadedFileName(applicant);
                applicant.PhotoUrl = uniqueFileName;
            }

            _context.Attach(applicant);
            _context.Entry(applicant).State = EntityState.Modified;

            _context.Experiences.AddRange(applicant.Experiences);

            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        private string GetUploadedFileName(Applicant applicant)
        {
            string uniqueFileName = null;
            if (applicant.ProfilePhoto !=null)
            {
                string uplodsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + applicant.ProfilePhoto.FileName;
                string filePath = Path.Combine(uplodsFolder, uniqueFileName);
                using (var fileStream = new FileStream (filePath, FileMode.Create))
                {
                    applicant.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult Details(int id)
        {
            Applicant applicant = _context.Applicants.Include(e => e.Experiences).Where(a => a.Id == id).FirstOrDefault();
            return View(applicant);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Applicant applicant = _context.Applicants.Include(e => e.Experiences).Where(a => a.Id == id).FirstOrDefault();
            return View(applicant);
        }

        [HttpPost]
        public IActionResult Delete(Applicant applicant)
        {
            _context.Attach(applicant);
            _context.Entry(applicant).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetGender()
        {
            List<SelectListItem> selGender = new List<SelectListItem>();
            var selItem = new SelectListItem()
            {
                Value = "",
                Text = "Select Gender"
            };

            selGender.Insert(0, selItem);
            selItem = new SelectListItem()
            {
                Value = "Male",
                Text = "Male"
            };

            selGender.Add(selItem);
            selItem = new SelectListItem()
            {
                Value = "Female",
                Text = "Female"
            };
            selGender.Add(selItem);
            return selGender;




        }

        //// GET: Resume
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Applicant != null ? 
        //                  View(await _context.Applicant.ToListAsync()) :
        //                  Problem("Entity set 'ResumeDbContext.Applicant'  is null.");
        //}

        //// GET: Resume/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Applicant == null)
        //    {
        //        return NotFound();
        //    }

        //    var applicant = await _context.Applicant
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (applicant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(applicant);
        //}

        //// GET: Resume/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Resume/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Gender,Age,Qualification,TotalExperiences,PhotoUrl")] Applicant applicant)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(applicant);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(applicant);
        //}

        //// GET: Resume/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Applicant == null)
        //    {
        //        return NotFound();
        //    }

        //    var applicant = await _context.Applicant.FindAsync(id);
        //    if (applicant == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(applicant);
        //}

        //// POST: Resume/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,Age,Qualification,TotalExperiences,PhotoUrl")] Applicant applicant)
        //{
        //    if (id != applicant.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(applicant);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ApplicantExists(applicant.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(applicant);
        //}

        //// GET: Resume/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Applicant == null)
        //    {
        //        return NotFound();
        //    }

        //    var applicant = await _context.Applicant
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (applicant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(applicant);
        //}

        //// POST: Resume/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Applicant == null)
        //    {
        //        return Problem("Entity set 'ResumeDbContext.Applicant'  is null.");
        //    }
        //    var applicant = await _context.Applicant.FindAsync(id);
        //    if (applicant != null)
        //    {
        //        _context.Applicant.Remove(applicant);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ApplicantExists(int id)
        //{
        //  return (_context.Applicant?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
