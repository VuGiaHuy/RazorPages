using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, GiaHuyDbContext dbContext) : base(roleManager, dbContext)
        {
        }

        public class Input
        {
            public string Name {get;set;} = default!;
        }
        [BindProperty]
        public Input input {get;set;}
        private IdentityRole role {get;set;}
        public async Task<IActionResult> OnGet(string roleid)
        {
            if(String.IsNullOrEmpty(roleid)) return NotFound();
            role = await _roleManager.FindByIdAsync(roleid)??default!;
            if(role!=null)
            {
                input = new Input()
                {
                    Name = role.Name!
                };
                return Page();
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPost(string roleid)
        {
            if(String.IsNullOrEmpty(roleid))  return NotFound();
            role = await _roleManager.FindByIdAsync(roleid)??default!;
            if(role == null) return NotFound();
            else
            {
               var result = await _roleManager.DeleteAsync(role);
               if(result.Succeeded) 
               {
                    statusMessage = $"{role.Name} delete successful!";
                    return RedirectToPage("./Index");
               }
               else
               {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();                    
               }
            }
        }
    }
}
