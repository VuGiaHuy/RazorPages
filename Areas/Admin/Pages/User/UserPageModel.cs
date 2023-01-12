using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiaHuy.User;
public class UserPageModel : PageModel
{
    protected readonly UserManager<AppUser> _userManager;
    protected readonly GiaHuyDbContext _dbContext;
    public UserPageModel(UserManager<AppUser> userManager, GiaHuyDbContext dbContext)
    {
        _userManager = userManager;

        _dbContext = dbContext;
    }
    [TempData]
    public string StatusMessage {get;set;} = default!;

}