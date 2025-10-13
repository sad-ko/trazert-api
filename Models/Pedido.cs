using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Models;

[PrimaryKey(nameof(Id))]
public class Pedido
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 1)]
    [JsonPropertyName("id")]
    public Int32 Id { get; set; }

    [JsonPropertyName("pedido")]
    public Int64 PedidoNro { get; set; }

    [JsonPropertyName("cliente")]
    public required string Cliente { get; set; }

    [JsonIgnore]
    public string? IdCtaAuxi { get; set; }
}