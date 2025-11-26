using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

public class PedidoDetalles
{
    [ForeignKey(nameof(PedidoId))]
    [JsonPropertyName("pedido_id")]
    public int PedidoId { get; init; }

    [JsonPropertyName("producto_id")]
    public int ProductoId { get; init; }

    [JsonPropertyName("descripcion")]
    [StringLength(40)]
    public required string Descripcion { get; init; }

    [JsonPropertyName("total")]
    [Precision(38,4)]
    public decimal Total { get; init; }

    [JsonPropertyName("ingresados")]
    [Precision(38,2)]
    public decimal Ingresados { get; init; }
}