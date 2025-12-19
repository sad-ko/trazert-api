using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

public class PalletDetalles
{
    [ForeignKey(nameof(PalletId))]
    [JsonPropertyName("pallet_id")]
    public int PalletId { get; init; }

    [JsonPropertyName("producto_id")]
    public int ProductoId { get; init; }

    [JsonPropertyName("descripcion")]
    [StringLength(40)]
    public required string Descripcion { get; init; }

    [JsonPropertyName("ingresados")]
    [Precision(38, 2)]
    public decimal Ingresados { get; init; }

    [JsonPropertyName("lote")]
    [StringLength(20)]
    public required string Lote { get; init; }

    [JsonPropertyName("cliente")]
    [StringLength(40)]
    public required string Cliente { get; init; }
}