using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, GiaHuyDbContext dbContext) : base(roleManager, dbContext)
        {
        }
        public List<IdentityRole> roles{get;set;}
        public async Task OnGetAsync()
        {
            roles = await _roleManager.Roles.OrderBy(identityRole=>identityRole.Name).ToListAsync();
        }
        public void OnPost() => RedirectToPage();
    }
}
