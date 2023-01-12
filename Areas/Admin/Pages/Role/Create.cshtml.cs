using System.ComponentModel.DataAnnotations;
using GiaHuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, GiaHuyDbContext dbContext) : base(roleManager, dbContext)
        {
        }
        public class Input
        {
            [Display(Name = "Role Name")]
            [Required(ErrorMessage = "You must type Role Name")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} length between {2} to {1} characters")]
            public string Name {get;set;} = default!; 
        }
        
        [BindProperty]
        public Input input {get;set;}
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid) return Page();

            var newRole = new IdentityRole(input.Name);
            var result = await _roleManager.CreateAsync(newRole);
            if(result.Succeeded)
            {
                StatusMessage = $"Create new role {input.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }
            return Page();
        }
    }
}
