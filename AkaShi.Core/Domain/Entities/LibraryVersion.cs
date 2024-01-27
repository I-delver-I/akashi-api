using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class LibraryVersion : BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }
    public int DownloadsCount { get; set; }
    
    public int? LibraryId { get; set; }
    [ForeignKey("LibraryId")]
    public Library Library { get; set; }
    
    public DateTime LastUpdateTime { get; set; }
    [MaxLength(6000)]
    public string UsageContent { get; set; }
    [MaxLength(255)]
    public string SourceRepositoryURL { get; set; }
    [MaxLength(255)]
    public string LicenseURL { get; set; }

    public int? FileExtensionId { get; set; }
    [ForeignKey("FileExtensionId")]
    public FileExtension FileExtension { get; set; }
    
    public ICollection<LibraryVersionDependency> LibraryVersionDependencies { get; private set; }
    public ICollection<LibraryVersionSupportedFramework> LibraryVersionSupportedFrameworks { get; private set; }
}