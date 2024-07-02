using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class LibraryVersionDependency : BaseEntity
{
    public int? FrameworkId { get; set; }
    [ForeignKey("FrameworkId")]
    public Framework Framework { get; set; }

    public int? LibraryVersionId { get; set; }
    [ForeignKey("LibraryVersionId")]
    public LibraryVersion LibraryVersion { get; set; }
    
    public int? DependencyLibraryId { get; set; }
    [ForeignKey("DependencyLibraryId")]
    public Library DependencyLibrary { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string SupportedVersions { get; set; }
}