using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDrop.Api.Core.Entities;

[Table("user_role")]
public class UserRoleEntity
{
    [Key]
    [Column("id", Order = 0)]
    public uint Id { get; set; }

    [Column("user_id")]
    public uint UserId { get; set; }

    [Column("role_id")]
    public uint RoleId { get; set; }


    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }


    // Navigation Properties

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(UserEntity.UserRoles))]
    public required UserEntity User { get; set; }

    [ForeignKey(nameof(RoleId))]
    [InverseProperty(nameof(RoleEntity.UserRoles))]
    public required RoleEntity Role { get; set; }
}
