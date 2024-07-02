using System.ComponentModel.DataAnnotations;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class FileExtension : BaseEntity
{
    [Required]
    [MaxLength(10)]
    public string Name { get; set; }

    public ICollection<Image> Images { get; private set; }
    public ICollection<LibraryVersion> LibraryVersions { get; private set; }
}