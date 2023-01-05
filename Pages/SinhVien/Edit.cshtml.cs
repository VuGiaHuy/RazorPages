using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiaHuy.Models;

namespace practice2.Pages_SinhVien
{
    public class EditModel : PageModel
    {
        private readonly GiaHuy.Models.GiaHuyDbContext _context;

        public EditModel(GiaHuy.Models.GiaHuyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SinhVien SinhVien { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.sinhVien == null)
            {
                return NotFound();
            }

            var sinhvien =  await _context.sinhVien.FirstOrDefaultAsync(m => m.Id == id);
            if (sinhvien == null)
            {
                return NotFound();
            }
            SinhVien = sinhvien;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SinhVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SinhVienExists(SinhVien.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SinhVienExists(int id)
        {
          return (_context.sinhVien?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
