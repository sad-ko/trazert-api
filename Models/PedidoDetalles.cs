using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Models;

public class PedidoDetalles
{
    [ForeignKey(nameof(PedidoId))]
    [JsonPropertyName("pedido_id")]
    public int PedidoId { get; set; }

    [JsonPropertyName("producto_id")]
    public int ProductoId { get; set; }

    [JsonPropertyName("descripcion")]
    public required string Descripcion { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [NotMapped]
    [JsonPropertyName("ingresados")]
    public int Ingresados { get; set; }
}