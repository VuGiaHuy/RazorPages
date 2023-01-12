using System.Linq;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GiaHuy.User
{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AddRoleModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        public AppUser user {get;set;}
        [BindProperty]
        [DisplayName("Role Names")]
        public string[] RoleNames {get;set;}
        public SelectList roleName {get;set;}
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(String.IsNullOrEmpty(id)) return NotFound();
            user = await _userManager.FindByIdAsync(id);
            if(user == null) return NotFound("k tim thay user");
            RoleNames = (await _userManager.GetRolesAsync(user)).ToArray<string>();
            List<string> roleNames = _roleManager.Roles.Select(role=>role.Name).ToList();
            roleName = new SelectList(roleNames);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if(String.IsNullOrEmpty(id)) return NotFound();
            user = await _userManager.FindByIdAsync(id);
            if(user == null) return NotFound("k tim thay user");

            var oldRoleName = (await _userManager.GetRolesAsync(user)).ToArray();
            var delRoles = oldRoleName.Where(role=>!RoleNames.Contains(role));
            var addRoles = RoleNames.Where(role=>!oldRoleName.Contains(role));   
            var resultRemove = await _userManager.RemoveFromRolesAsync(user, delRoles);
            List<string> roleNames = _roleManager.Roles.Select(role=>role.Name).ToList();
            roleName = new SelectList(roleNames);
            if(!resultRemove.Succeeded)
            {
                foreach(var error in resultRemove.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if(!resultAdd.Succeeded)
            {
                foreach(var error in resultAdd.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
                return Page();
            }

            StatusMessage = "Update Role Successful";
            return RedirectToPage("./Index");
        }
    }
}
