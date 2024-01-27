using System.ComponentModel.DataAnnotations;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class User : BaseEntity
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }
    [Required]
    [MaxLength(64)]
    public string PasswordHash { get; set; }
    [Required]
    [MaxLength(128)]
    public string PasswordSalt { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(320, MinimumLength = 3)]
    public string Email { get; set; }
    
    public ICollection<Library> Libraries { get; private set; }
}