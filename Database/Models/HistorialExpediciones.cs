using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

public class HistorialExpediciones
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("pedido_id")]
    public int? PedidoId { get; init; }

    [JsonPropertyName("estado")]
    public char Estado { get; init; }

    [JsonPropertyName("fechaHoraCreacion")]
    public DateTime FechaHoraCreacion { get; init; }

    [JsonPropertyName("fechaHoraCierre")]
    public DateTime? FechaHoraCierre { get; init; }

    [JsonPropertyName("cliente")]
    [StringLength(40)]
    public required string Cliente { get; init; }
}