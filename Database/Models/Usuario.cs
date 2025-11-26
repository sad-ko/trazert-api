using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

[PrimaryKey(nameof(Id))]
public class Usuario
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 1)]
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("nombre")]
    [StringLength(14)]
    public required string Nombre { get; init; }

    [JsonPropertyName("fecha_baja")]
    public DateTime? FechaBaja { get; init; }
    
    [JsonIgnore]
    [StringLength(97)]
    public required string Hash { get; init; }
}