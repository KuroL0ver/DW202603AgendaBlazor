using AgendaWeb.Data.Commands;
using AgendaWeb.Data.DTOS.TiposContactos;
using AgendaWeb.Data.Entities;

namespace AgendaWeb.Services
{
    public class TipoContactoServices
    {
        private readonly TipoContactoCommand _tipoContactoCommand;
        

        public TipoContactoServices(TipoContactoCommand tipoContactoCommand)
        {
            _tipoContactoCommand = tipoContactoCommand;
            
        }

        public async Task InsertarAsync(TipoContactoCrearDto dto)
        {
            TipoContacto entity = new TipoContacto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            int rows = await _tipoContactoCommand.InsertarTipoContactoAsync(entity);
            if (rows == 0)
            {
                throw new Exception("No se pudo insertar el tipo de contacto.");
            }
        }

        public async Task<int> EliminarAsync(int id)
        {
            return await _tipoContactoCommand.EliminarTipoContactoAsync(id);
        }
        public async Task<TipoContactoDto> ObtenerPorIdAsync(int id)
        {
            var entity = await _tipoContactoCommand.ObtenerPorIdAsync(id);
            if (entity == null) return null;

            return new TipoContactoDto
            {
                Id = (int)entity.Id,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion
            };
        }

        public async Task ActualizarTipoContactoAsync(TipoContactoDto dto)
        {
            var entity = await _tipoContactoCommand.ObtenerPorIdAsync(dto.Id);
            if (entity != null)
            {
                entity.Nombre = dto.Nombre;
                entity.Descripcion = dto.Descripcion;
                await _tipoContactoCommand.ActualizarTipoContactoAsync(dto.Id, entity);
            }
        }
        public async Task<int> ActualizarAsync(int id, TipoContactoActualizarDto dto)
        {
            TipoContacto entity = new TipoContacto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };
            return await _tipoContactoCommand.ActualizarTipoContactoAsync(id, entity);
        }

        public async Task<List<TipoContactoDto>> ObtenerTodosAsync()
        {
            // El comando ya devuelve una lista de TipoContactoDto (desde AgendaWeb.Services), reutilizarla directamente
            var tipos = await _tipoContactoCommand.ObtenerTodosAsync();
            return tipos;
        }

        public async Task InsertarTipoContactoAsync(AgendaWeb.Data.Entities.TipoContacto entity) 
        
        {
         await _tipoContactoCommand.InsertarTipoContactoAsync(entity);
        }
    }
}
