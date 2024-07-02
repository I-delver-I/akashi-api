using System.ComponentModel.DataAnnotations;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class Framework : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string ProductName { get; set; }
    [Required]
    [MaxLength(20)]
    public string VersionName { get; set; }
    
    public ICollection<LibraryVersionDependency> LibraryVersionDependencies { get; private set; }
    public ICollection<LibraryVersionSupportedFramework> LibraryVersionSupportedFrameworks { get; private set; }
}