using System.ComponentModel.DataAnnotations;

namespace NioZero.Data.Entities;

public class BlogEntry : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Code { get; set; }

    public int Status { get; set; }

    public DateTimeOffset PublishedDate { get; set; }

    public DateTimeOffset LastUpdatedDate { get; set; }
}
