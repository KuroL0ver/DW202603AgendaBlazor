namespace AgendaWeb.Data.DTOS.Contactos
{
    public class ContactoActualizarDto
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int? TipoContactoId { get; set; }
    }
}
