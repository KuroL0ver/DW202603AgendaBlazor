namespace AgendaWeb.Data.Entities;
public class Contacto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    // Relación opcional con TipoContacto
    public int? TipoContactoId { get; set; }
    public string? TipoContactoDescripcion { get; set; }
}
