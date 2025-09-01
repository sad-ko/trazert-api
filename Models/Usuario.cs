using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Models;

[PrimaryKey(nameof(Id))]
public class Usuario
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 1)]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public required string Nombre { get; set; }

    [JsonPropertyName("fecha_baja")]
    public DateTime? FechaBaja { get; set; }

    [Column(TypeName = "char")]
    [MaxLength(24)]
    [JsonIgnore]
    public required string Hash { get; set; }
}