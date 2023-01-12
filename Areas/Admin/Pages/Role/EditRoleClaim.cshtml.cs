using System.Security.Claims;
using System.Net;
using System.ComponentModel.DataAnnotations;
using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Authorize(Policy = "Admin")]
    public class EditRoleClaim : RolePageModel
    {
        public EditRoleClaim(RoleManager<IdentityRole> roleManager,
         GiaHuyDbContext dbContext
         ) : base(roleManager, dbContext)
        {
        }
        public class Input
        {
            [Display(Name = "Claim Name")]
            [Required(ErrorMessage = "You must type Role Name")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} length between {2} to {1} characters")]
            public string ClaimName {get;set;} = default!; 

            [Display(Name = "Claim Value")]
            [Required(ErrorMessage = "You must type Role Name")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} length between {2} to {1} characters")]
            public string ClaimValue {get;set;} = default!; 
        }
        [BindProperty]
        public Input input {get;set;}
        public IdentityRoleClaim<string> Claim {set;get;}
        public IdentityRole role {get;set;} 
        public async Task<IActionResult> OnGetAsync(int? claimid)
        {
            if(claimid == null) return NotFound();
            Claim = _dbContext.RoleClaims.Where(c=>c.Id == claimid).FirstOrDefault()??default!;
            if(Claim==null) return NotFound();
            role = await _roleManager.FindByIdAsync(Claim.RoleId)??default!;
            if(role == null) return NotFound();
            input = new Input()
            {
                ClaimName = Claim.ClaimType!,
                ClaimValue = Claim.ClaimValue!
            };
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? claimid)
        {
            if(claimid == null) return NotFound();
            if(!ModelState.IsValid) return Page();

            Claim = _dbContext.RoleClaims.Where(c=>c.Id==claimid).FirstOrDefault()!;
            if(Claim == null) return NotFound();

            role = await _roleManager.FindByIdAsync(Claim.RoleId)??default!;
            if(role==null) return NotFound();
            if(_dbContext.RoleClaims.Any(c=>c.RoleId==role.Id && c.Id != Claim.Id && c.ClaimType == Claim.ClaimType && c.ClaimValue==Claim.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Role have already exists");
                return Page();
            }
            Claim.ClaimType = input.ClaimName;
            Claim.ClaimValue = input.ClaimValue;
            await _dbContext.SaveChangesAsync();
            StatusMessage = $"Update claim ({Claim.ClaimType}) success";
            return RedirectToPage("./Edit",new{roleid = role.Id});
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if(claimid == null) return NotFound("khong tim thay");

            Claim = _dbContext.RoleClaims.Where(c=>c.Id==claimid).FirstOrDefault()!;
            if(Claim == null) return NotFound();

            role = await _roleManager.FindByIdAsync(Claim.RoleId)??default!;
            if(role==null) return NotFound();
            var claim = new Claim(input.ClaimName,input.ClaimValue);
            await _roleManager.RemoveClaimAsync(role, claim);
            StatusMessage = $"Delete claim ({Claim.ClaimType}) success";
            return RedirectToPage("./Edit",new{roleid = role.Id});
        }
    }
}
