using System.ComponentModel.DataAnnotations.Schema;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class LibraryVersionSupportedFramework : BaseEntity
{
    public int? LibraryVersionId { get; set; }
    [ForeignKey("LibraryVersionId")]
    public LibraryVersion LibraryVersion { get; set; }

    public int? FrameworkId { get; set; }
    [ForeignKey("FrameworkId")]
    public Framework Framework { get; set; }
}