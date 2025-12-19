using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

public class ListarPallets
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("codbar")]
    [StringLength(8)]
    public required string Codbar { get; init; }

    [JsonPropertyName("estado")]
    public bool Estado { get; init; }

    [JsonPropertyName("mono_producto")]
    public bool MonoProducto { get; init; }
}