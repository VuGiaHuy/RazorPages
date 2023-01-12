using GiaHuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager,
        GiaHuyDbContext dbContext) : base(roleManager, dbContext)
        {
        }
        public class RoleModel : IdentityRole
        {
            public string[] Claims{get;set;} = default!;
        }
        public List<RoleModel> roles{get;set;}
        public async Task OnGetAsync()
        {
            var r = await _roleManager.Roles.OrderBy(identityRole=>identityRole.Name).ToListAsync();
            roles = new List<RoleModel>();
            foreach(var role in r)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var claimString = claims.Select(claim=>claim.Type +"="+ claim.Value);
                
                var rm = new RoleModel(){
                    Name = role.Name,
                    Id = role.Id,
                    Claims = claimString.ToArray()
                };
                roles.Add(rm);
            }
        }
        public void OnPost() => RedirectToPage();
    }
}
