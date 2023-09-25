using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDrop.Api.Core.Entities;

[Table("role")]
[Index(nameof(Name), Name = "UK_role_name", IsUnique = true)]
public class RoleEntity : Base.BaseEntity
{
    [Column("name")]
    [StringLength(100)]
    public required string Name { get; set; }

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }


    [Column("is_active")]
    public bool? IsActive { get; set; }

    // Navigation Properties

    [InverseProperty(nameof(UserRoleEntity.Role))]
    public virtual ICollection<UserRoleEntity> UserRoles { get; set; } = new HashSet<UserRoleEntity>();
}
