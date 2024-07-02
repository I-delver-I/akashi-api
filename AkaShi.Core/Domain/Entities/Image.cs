using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public sealed class Image : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string URL { get; set; }
    public long Timestamp { get; set; }

    public int? FileExtensionId { get; set; }
    [ForeignKey("FileExtensionId")]
    public FileExtension FileExtension { get; set; }
    
    public ICollection<Library> Libraries { get; private set; }
}