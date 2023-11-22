using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDrop.Api.Core.Entities;

[Table("user_type")]
[Index(nameof(Name), Name = "UK_user_type_name", IsUnique = true)]
public class UserTypeEntity : Base.BaseEntity
{
    [Column("name")]
    [StringLength(50)]
    public required string Name { get; set; }

    [Column("description")]
    [StringLength(50)]
    public string? Description { get; set; }


    [Column("is_active")]
    public bool? IsActive { get; set; }

    // Navigation Properties

    [InverseProperty(nameof(UserEntity.Type))]
    public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();
}
