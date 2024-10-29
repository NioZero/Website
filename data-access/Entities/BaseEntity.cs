using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class BaseEntity
{
    [Key]
    public long ID { get; set; }

    public bool Active { get; set; }

    public bool Deleted { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset ModifiedDate { get; set; }
}
