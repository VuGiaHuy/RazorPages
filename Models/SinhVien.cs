using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace GiaHuy.Models;
public class SinhVien
{
    [Key]
    public int Id {get;set;}
    [Required]
    [StringLength(255)]
    [Column(TypeName ="nvarchar")]
    public string Name {get;set;}
    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDay {get;set;}
    [Required]
    [Column(TypeName = "ntext")]
    public string Address {get;set;}
}