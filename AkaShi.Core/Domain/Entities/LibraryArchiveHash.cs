using System.ComponentModel.DataAnnotations;
using AkaShi.Core.Domain.Entities.Abstract;

namespace AkaShi.Core.Domain.Entities;

public class LibraryArchiveHash : BaseEntity
{
    [Required]
    [MaxLength(64)]
    public string Hash { get; set; }
}