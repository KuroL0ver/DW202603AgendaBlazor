using AgendaWeb.Data.Commands;
using AgendaWeb.Data.DTOS.Contactos;
using AgendaWeb.Data.Entities;

namespace AgendaWeb.Services
{
    public class ContactoServices
    {
        private readonly ContactoCommand _contactoCommand;

        public ContactoServices(ContactoCommand contactoCommand)
        {
            _contactoCommand = contactoCommand;
        }

        public async Task InsertarAsync(ContactoNuevoDto contactoNuevoDto)
        {
            Contacto contacto = new Contacto();
            contacto.Nombre = contactoNuevoDto.Nombre;
            contacto.Telefono = contactoNuevoDto.Telefono;
            contacto.Email = contactoNuevoDto.Email;
            contacto.TipoContactoId = contactoNuevoDto.TipoContactoId;

            int registrosAfectados = await _contactoCommand.InsertarContactoAsync(contacto);

            if (registrosAfectados == 0)
            {
                throw new Exception("No se pudo insertar el contacto.");
            }
        }
        public async Task<int> EliminarContactoAsync(int id)
        {
            return await _contactoCommand.EliminarContactoAsync(id);
        }

        public async Task<ContactoDto?> ObtenerPorIdAsync(int id)
        {
            var c = await _contactoCommand.ObtenerPorIdAsync(id);
            if (c == null) return null;
            return new ContactoDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Telefono = c.Telefono,
                Email = c.Email,
                TipoContactoDescripcion = c.TipoContactoDescripcion,
                TipoContactoId = c.TipoContactoId
            };
        }

        public async Task<int> ActualizarContactoAsync(int id, ContactoActualizarDto contactoActualizarDto)
        {
            Contacto contacto = new Contacto
            {
                Nombre = contactoActualizarDto.Nombre,
                Telefono = contactoActualizarDto.Telefono,
                Email = contactoActualizarDto.Email
                ,TipoContactoId = contactoActualizarDto.TipoContactoId
            };
            return await _contactoCommand.ActualizarContactoAsync(id, contacto);
        }

        public async Task<int> ActualizarSoloTipoContactoAsync(int id, int? tipoContactoId)
        {
            return await _contactoCommand.ActualizarTipoContactoAsync(id, tipoContactoId);
        }

        public async Task<List<ContactoDto>> ObtenerTodosAsync()
        {
            List<Contacto> contactos = await _contactoCommand.ObtenerTodosAsync();

            List<ContactoDto> contactosDto = new List<ContactoDto>();

            foreach (var c in contactos)
            {
                ContactoDto contactoDto = new ContactoDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Telefono = c.Telefono,
                    Email = c.Email
                    ,TipoContactoDescripcion = c.TipoContactoDescripcion
                
                };
                contactosDto.Add(contactoDto);
            }


            return contactosDto;
        }
    }
}
