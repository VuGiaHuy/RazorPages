using Microsoft.AspNetCore.Mvc;
using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MyApp.Namespace;
public class RolePageModel : PageModel
{
    protected readonly RoleManager<IdentityRole> _roleManager;
    protected readonly GiaHuyDbContext _dbContext;
    
    [TempData]
    public string statusMessage {get;set;}= default!;
    public RolePageModel(RoleManager<IdentityRole> roleManager, GiaHuyDbContext dbContext)
    {
        _roleManager = roleManager;

        _dbContext = dbContext;
    }
}