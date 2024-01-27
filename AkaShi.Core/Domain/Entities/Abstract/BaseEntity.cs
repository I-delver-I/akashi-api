using System.ComponentModel.DataAnnotations;

namespace AkaShi.Core.Domain.Entities.Abstract;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}