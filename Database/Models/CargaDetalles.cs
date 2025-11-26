using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

public class CargaDetalles
{
    [ForeignKey(nameof(ExpedicionId))]
    [JsonPropertyName("expedicion_id")]
    public int ExpedicionId { get; init; }

    [JsonPropertyName("producto_id")]
    public int ProductoId { get; init; }

    [JsonPropertyName("descripcion")]
    [StringLength(40)]
    public required string Descripcion { get; init; }

    [JsonPropertyName("ingresados")]
    [Precision(38,2)]
    public decimal Ingresados { get; init; }
}