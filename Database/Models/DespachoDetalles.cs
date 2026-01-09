using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

public class DespachoDetalles
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

    [JsonPropertyName("producto_id")]
    public int ProductoId { get; init; }

    [JsonPropertyName("descripcion")]
    [StringLength(40)]
    public required string Descripcion { get; init; }

    [JsonPropertyName("ingresados")]
    [Precision(38, 2)]
    public decimal Ingresados { get; init; }

    [JsonPropertyName("total")]
    [Precision(38, 4)]
    public decimal? Total { get; init; }

    [JsonPropertyName("operador")]
    [StringLength(14)]
    public required string Operador { get; init; }

    [JsonPropertyName("puesto")]
    [StringLength(50)]
    public required string Puesto { get; init; }
}