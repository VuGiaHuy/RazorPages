using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace GiaHuy.User
{
    public class EditUserClaimModel : PageModel
    {
        private readonly GiaHuyDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        public EditUserClaimModel(GiaHuyDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage {get;set;}
        public async Task<IActionResult> OnGet()
        {
            return NotFound();
        }
        [BindProperty]
        public Input input {get;set;}
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
        public AppUser user {get;set;}
        public async Task<IActionResult> OnGetAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if(user == null) return NotFound();
            return Page();
        }
        public IdentityUserClaim<string> userClaim{set;get;}
        public async Task<IActionResult> OnGetEditClaimAsync(int? claimid)
        {
            if(claimid==null) return NotFound();
            userClaim = (_dbContext.UserClaims.Where(c=>c.Id == claimid)).FirstOrDefault();
            user = await _userManager.FindByIdAsync(userClaim.UserId)??default!;
            if(user==null) return NotFound();
            input = new Input(){
                ClaimType = userClaim.ClaimType!,
                ClaimValue = userClaim.ClaimValue!
            }; 
            return Page();
        }
        public async Task<IActionResult> OnPostEditClaimAsync(int? claimid)
        {
            if(claimid==null) return NotFound();
            userClaim = _dbContext.UserClaims.Where(c=>c.Id==claimid).FirstOrDefault()??default!;
            if(userClaim==null) return NotFound();
            user = await _userManager.FindByIdAsync(userClaim.UserId)??default!;
            if(user==null) return NotFound();
            if(!ModelState.IsValid) return Page();
            if(_dbContext.UserClaims.Any(c=>c.ClaimType==input.ClaimType&&c.ClaimValue==input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty,"Claim have already exists");
                return Page();
            }
            userClaim.ClaimType = input.ClaimType;
            userClaim.ClaimValue = input.ClaimValue;
            await _dbContext.SaveChangesAsync();
            StatusMessage = $"Update {input.ClaimType} success";
            return RedirectToPage("./AddRole",new {id=user.Id});
            
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if(claimid==null) return NotFound();
            userClaim = _dbContext.UserClaims.Where(c=>c.Id==claimid).FirstOrDefault()??default!;
            if(userClaim==null) return NotFound();
            user = await _userManager.FindByIdAsync(userClaim.UserId)??default!;
            if(user==null) return NotFound();
            if(!ModelState.IsValid) return Page();

            
            var result = await _userManager.RemoveClaimAsync(user, new Claim(input.ClaimType,input.ClaimValue));
            if(result.Succeeded)
            {
                StatusMessage = $"Delete {input.ClaimType} success";
                return RedirectToPage("./AddRole",new {id=user.Id});
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                    return Page();
                }
            }
            return Page();
            
        }
        public async Task<IActionResult> OnPostAddClaimAsync(string userid)
        {
            if(string.IsNullOrEmpty(userid)) return NotFound();
            user = await _userManager.FindByIdAsync(userid);
            if(user == null) return NotFound();
            if(!ModelState.IsValid) return Page();
            var claims = _dbContext.UserClaims.Where(c=>c.UserId == userid);
            if(claims.Any(c=>c.ClaimType==input.ClaimType && c.ClaimValue==input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Claim have already exists");
                return Page();
            }
            var result = await _userManager.AddClaimAsync(user,new Claim(input.ClaimType,input.ClaimValue));
            if(result.Succeeded)
            {
                StatusMessage = $"Add User Claim {input.ClaimType} success";
                return RedirectToPage("./AddRole",new {id=user.Id});
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                    return Page();
                }
            }
            return Page();
        }
    }
}
