using GiaHuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiaHuy.User
{
    public class IndexModel : UserPageModel
    {
        public int totalUser {get;set;}
        public int countPages {get;set;}
        [BindProperty(SupportsGet = true, Name = "pages")]
        public int currentPage {get;set;}
        private const int Items_Per_Page = 5;
        public IndexModel(UserManager<AppUser> userManager, GiaHuyDbContext dbContext) : base(userManager, dbContext)
        {
        }
        public class UserAndRole : AppUser
        {
            public string RoleNames {get;set;} = default!;
        }
        public List<UserAndRole> users {get;set;}
        public async Task OnGet(int pages)
        {
            var qr = _userManager.Users.OrderBy(user=>user.UserName);
            totalUser = await qr.CountAsync();
            countPages =(int)Math.Round((double) totalUser/Items_Per_Page);
            if(currentPage<1) currentPage = 1;
            if(currentPage>countPages) currentPage = countPages;

            var qr2 = qr.Skip(Items_Per_Page*(currentPage-1)).Take(Items_Per_Page)
                        .Select(user=>new UserAndRole(){
                            Id = user.Id,
                            UserName = user.UserName
                        });
            users = await qr2.ToListAsync();
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames = string.Join(",", roles);
            }
        }
    }
}
