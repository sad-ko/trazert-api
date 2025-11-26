using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Database.Models;

[PrimaryKey(nameof(Id))]
public class Pedido
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 1)]
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("pedido")]
    public long PedidoNro { get; init; }

    [JsonPropertyName("cliente")]
    [StringLength(40)]
    public required string Cliente { get; init; }

    [JsonIgnore]
    [StringLength(12)]
    public string? IdCtaAuxi { get; init; }
}