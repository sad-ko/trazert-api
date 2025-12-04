using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

public class Cliente
{
    [JsonPropertyName("idAuxi")]
    public required short IdAuxi { get; init; }
    
    [JsonPropertyName("idCtaAuxi")]
    [StringLength(12)]
    public required string IdCtaAuxi { get; init; }

    [JsonPropertyName("nombre")]
    [StringLength(40)]
    public required string Nombre { get; init; }
}