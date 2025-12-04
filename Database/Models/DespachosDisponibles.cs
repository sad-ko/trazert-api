using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

[PrimaryKey(nameof(ExpedicionId))]
public class Despacho
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 1)]
    [JsonPropertyName("id")]
    public int ExpedicionId { get; init; }
    
    [JsonPropertyName("idAuxi")]
    public required short IdAuxi { get; init; }

    [JsonPropertyName("idCtaAuxi")]
    [StringLength(12)]
    public required string IdCtaAuxi { get; init; }

    [JsonPropertyName("cliente")]
    [StringLength(40)]
    public required string Cliente { get; init; }
}