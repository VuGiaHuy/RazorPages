using System.Net;
using System.ComponentModel.DataAnnotations;
using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Namespace
{
    [Authorize(Policy = "Admin")]
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, GiaHuyDbContext dbContext) : base(roleManager, dbContext)
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
        public IdentityRole role {get;set;} 
        public async Task<IActionResult> OnGet(string roleid)
        {
            if(String.IsNullOrEmpty(roleid)) return NotFound();
            role = await _roleManager.FindByIdAsync(roleid)??default!;
            if(role!=null)
            {
                input = new Input(){
                    Name = role.Name!
                };
                return Page();
            }
            return NotFound();
        }
        public async Task<IActionResult> OnPost(string roleid)
        {
            if(String.IsNullOrEmpty(roleid)) return NotFound();
            role = await _roleManager.FindByIdAsync(roleid)??default!;
            if(role == null)
            {
                return NotFound();
            }
            if(!ModelState.IsValid) return NotFound();

            role.Name = input.Name;
            var result = await _roleManager.UpdateAsync(role);
            if(result.Succeeded)
            {
                StatusMessage = $"{input.Name} update successful";
                return RedirectToPage("./Index");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);                    
                }
                return Page();
            }
        }
    }
}
