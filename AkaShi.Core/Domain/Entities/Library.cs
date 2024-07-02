using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class Library : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    public int? UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public int DownloadsCount { get; set; }
    public DateTime LastUpdateTime { get; set; }
    [MaxLength(200)]
    public string ShortDescription { get; set; }
    [MaxLength(4000)]
    public string Tags { get; set; }
    [MaxLength(255)]
    public string ProjectWebsiteURL { get; set; }
    
    public int? LogoId { get; set; }
    [ForeignKey("LogoId")]
    public Image Logo { get; set; }
    
    public ICollection<LibraryVersion> LibraryVersions { get; private set; }
    public ICollection<LibraryVersionDependency> LibraryVersionDependencies { get; private set; }
}