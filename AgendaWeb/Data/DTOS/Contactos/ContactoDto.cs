namespace AgendaWeb.Data.DTOS.Contactos
{
    public class ContactoDto
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string? TipoContactoDescripcion { get; set; }
        public int? TipoContactoId { get; internal set; }
    }
}
