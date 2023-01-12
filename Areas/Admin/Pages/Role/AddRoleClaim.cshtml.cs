using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using GiaHuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    [Authorize(Policy = "Admin")]
    public class AddRoleClaim : RolePageModel
    {
        public AddRoleClaim(RoleManager<IdentityRole> roleManager, GiaHuyDbContext dbContext) : base(roleManager, dbContext)
        {
        }
        public class Input
        {
            [Display(Name = "Claim Name")]
            [Required(ErrorMessage = "You must type Claim Name")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0}length between {2} to {1} characters")]
            public string ClaimType {get;set;} = default!;

            [Display(Name = "Claim Value")]
            [Required(ErrorMessage = "You must type Claim Name")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0}length between {2} to {1} characters")]
            public string ClaimValue {get;set;} = default!; 
        }
        
        [BindProperty]
        public Input input {get;set;}
        public IdentityRole role {get;set;}
        public async Task<IActionResult> OnGet(string roleid)
        {
            role = await _roleManager.FindByIdAsync(roleid)??default!;
            if(role == null) return NotFound();
            return Page();
        }
        public async Task<IActionResult> OnPost(string roleid)
        {
            if(string.IsNullOrEmpty(roleid)) return NotFound();
            role = await _roleManager.FindByIdAsync(roleid)??default!;
            if(role==null) return NotFound();
            if(!ModelState.IsValid) return Page();

            var result = (await _roleManager.GetClaimsAsync(role))
            .Any(c=>c.Type == input.ClaimType && c.Value==input.ClaimValue);
            if(result)
            {
                ModelState.AddModelError(string.Empty, "Claim already exists");
                return Page();
            }
            else
            {
                var claim = new Claim(input.ClaimType,input.ClaimValue);
                var resultAdd = await _roleManager.AddClaimAsync(role,claim);
                if(resultAdd.Succeeded)
                {
                    StatusMessage = $"Create {input.ClaimType} for {role.Name} successful!";
                    return RedirectToPage("./Edit",new {roleid=role.Id});
                }
                else
                {
                    foreach(var error in resultAdd.Errors)
                    {
                        ModelState.AddModelError(string.Empty,error.Description);
                        return Page();
                    }
                }
            }
            return Page();
        }
    }
}
