using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GiaHuy.Models;

namespace practice2.Pages_SinhVien
{
    public class DeleteModel : PageModel
    {
        private readonly GiaHuy.Models.GiaHuyDbContext _context;

        public DeleteModel(GiaHuy.Models.GiaHuyDbContext context)
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

            var sinhvien = await _context.sinhVien.FirstOrDefaultAsync(m => m.Id == id);

            if (sinhvien == null)
            {
                return NotFound();
            }
            else 
            {
                SinhVien = sinhvien;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.sinhVien == null)
            {
                return NotFound();
            }
            var sinhvien = await _context.sinhVien.FindAsync(id);

            if (sinhvien != null)
            {
                SinhVien = sinhvien;
                _context.sinhVien.Remove(SinhVien);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
