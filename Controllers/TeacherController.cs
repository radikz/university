using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using university.Models;

namespace university.Controllers
{
    public class TeacherController : Controller
    {
        private readonly universityContext _context;

        public TeacherController(universityContext context)
        {
            _context = context;
        }

        // GET: Teacher
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TotalStudentParm"] = sortOrder == "total" ? "total_desc" : "total";
            ViewData["SalaryParm"] = sortOrder == "salary" ? "salary_desc" : "salary";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            var teacher = from s in _context.Teacher
                        select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                teacher = teacher.Where(s => s.Name.Contains(searchString)
                                    || s.Skills.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "total_desc":
                    teacher = teacher.OrderByDescending(s => s.TotalStudents);
                    break;
                case "salary_desc":
                    teacher = teacher.OrderByDescending(s => s.Salary);
                    break;
                case "salary":
                    teacher = teacher.OrderBy(s => s.Salary);
                    break;
                case "Date":
                    teacher = teacher.OrderBy(s => s.AddedOn);
                    break;
                case "date_desc":
                    teacher = teacher.OrderByDescending(s => s.AddedOn);
                    break;
                case "total":
                    teacher = teacher.OrderBy(s => s.TotalStudents);
                    break;
            }
            return View(await teacher.AsNoTracking().ToListAsync());
        }

        // GET: Teacher/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teacher/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Skills,TotalStudents,Salary,AddedOn")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teacher/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Skills,TotalStudents,Salary,AddedOn")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            return View(teacher);
        }

        // GET: Teacher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }
    }
}
