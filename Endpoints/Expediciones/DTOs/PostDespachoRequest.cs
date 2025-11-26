namespace Trazert_API.Endpoints.Expediciones.DTOs;

public record PostDespachoRequest(bool Agregar, int? Expedicion, string CtaAuxi, string Codbar);