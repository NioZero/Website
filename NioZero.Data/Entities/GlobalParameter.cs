using System.ComponentModel.DataAnnotations;

namespace NioZero.Data.Entities;

public class GlobalParameter : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Key { get; set; }

    [Required]
    [StringLength(4000)]
    public string Value { get; set; }
}
