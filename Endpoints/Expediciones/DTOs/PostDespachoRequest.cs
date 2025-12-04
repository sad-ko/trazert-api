namespace Trazert_API.Endpoints.Expediciones.DTOs;

public record PostDespachoRequest(bool Agregar, int? Expedicion, short IdAuxi, string CtaAuxi, string Codbar);