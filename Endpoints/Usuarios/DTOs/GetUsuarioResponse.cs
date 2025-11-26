namespace Trazert_API.Endpoints.Usuarios.DTOs;

public class GetUsuarioResponse(int id, string nombre)
{
    public int Id { get; } = id;
    public string Nombre { get; } = nombre;
}