using GiaHuy.Models;
public class Paging
{
    public int currentPage {get;set;}
    public int countPages {get;set;}
    public Func<int?,string> generateUrl {get;set;} = default!;
}
