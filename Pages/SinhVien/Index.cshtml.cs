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
    public class IndexModel : PageModel
    {
        private readonly GiaHuy.Models.GiaHuyDbContext _context;
        public int Items_Per_Page = 5;
        [BindProperty(SupportsGet = true,Name = "pages")]
        public int currentPage { get; set; }
        public int countPages { get; set; }

        public IndexModel(GiaHuy.Models.GiaHuyDbContext context)
        {
            _context = context;
        }

        public IList<SinhVien> SinhVien { get; set; } = default!;

        public async Task OnGetAsync(string? name)
        {
            if (_context.sinhVien != null)
            {
                int totalPage = _context.sinhVien.ToList().Count();
                countPages = (int)Math.Floor((double)totalPage / Items_Per_Page);
                if (currentPage < 1) currentPage = 1;
                if (currentPage > countPages) currentPage = countPages;
                if (!string.IsNullOrEmpty(name))
                {
                    name = name.Trim();
                    var qr = (from sinhvien in _context.sinhVien
                              where sinhvien.Name.Contains(name)
                              select sinhvien).Skip((currentPage-1)*Items_Per_Page).Take(Items_Per_Page).ToListAsync();
                    SinhVien = await qr;
                }
                else SinhVien = await _context.sinhVien.Skip((currentPage-1)*Items_Per_Page).Take(Items_Per_Page).ToListAsync();
            }
        }
    }
}
