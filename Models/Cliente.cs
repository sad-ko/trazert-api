using System.Text.Json.Serialization;

namespace Trazert_API.Models;

public class Cliente
{
    [JsonPropertyName("idCtaAuxi")]
    public required string IdCtaAuxi { get; set; }

    [JsonPropertyName("nombre")]
    public required string Nombre { get; set; }
}