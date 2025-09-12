using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trazert_API.Models;

[PrimaryKey(nameof(ExpedicionId))]
public class Despacho
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 1)]
    [JsonPropertyName("expedicion_id")]
    public int ExpedicionId { get; set; }

    [JsonPropertyName("idCtaAuxi")]
    public required string IdCtaAuxi { get; set; }

    [JsonPropertyName("cliente")]
    public required string Cliente { get; set; }
}