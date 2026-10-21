using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Server.Models;

[Table("CATEGORIES")]
public class Category
{
    [Key]
    public int CategoryID { get; set; }

    [Required]
    public int EventID { get; set; }

    [Required, StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(50)]
    public string? CategoryType { get; set; }

    [StringLength(50)]
    public string? CategoryValue { get; set; }
}