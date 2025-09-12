using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Models;

public class CargaDetalles
{
    [ForeignKey(nameof(ExpedicionId))]
    [JsonPropertyName("expedicion_id")]
    public int ExpedicionId { get; set; }

    [JsonPropertyName("producto_id")]
    public int ProductoId { get; set; }

    [JsonPropertyName("descripcion")]
    public required string Descripcion { get; set; }

    [JsonPropertyName("ingresados")]
    public decimal Ingresados { get; set; }
}