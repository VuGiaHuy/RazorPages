using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GiaHuy.Models;

namespace practice2.Pages_SinhVien
{
    public class CreateModel : PageModel
    {
        private readonly GiaHuy.Models.GiaHuyDbContext _context;

        public CreateModel(GiaHuy.Models.GiaHuyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SinhVien SinhVien { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.sinhVien == null || SinhVien == null)
            {
                return Page();
            }

            _context.sinhVien.Add(SinhVien);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
