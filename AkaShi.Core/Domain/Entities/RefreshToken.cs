using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class RefreshToken : BaseEntity
{
    private const int DaysToExpire = 1;

    [Required]
    [MaxLength(256)]
    public string Token { get; set; }
    public DateTime Expires { get; } = DateTime.UtcNow.AddDays(DaysToExpire);

    public int? UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    [NotMapped]
    public bool IsActive => DateTime.UtcNow <= Expires;
}