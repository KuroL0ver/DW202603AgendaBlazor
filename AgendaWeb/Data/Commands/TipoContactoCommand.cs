using AgendaWeb.Data.Entities;
using AgendaWeb.Models;
using AgendaWeb.Services;
using AgendaWeb.Data.Entities;
using Microsoft.Data.SqlClient;

namespace AgendaWeb.Data.Commands
{
    public class TipoContactoCommand
    {
        private readonly SQLServer _sqlServer;

        public TipoContactoCommand(SQLServer sqlServer)
        {
            _sqlServer = sqlServer;
        }

        public async Task<int> InsertarTipoContactoAsync(TipoContactoDto tipoContactodto)
        {
            string query = "INSERT INTO TiposContactos (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", tipoContactodto.Nombre),
                new SqlParameter("@Descripcion", (object?)tipoContactodto.Descripcion ?? DBNull.Value)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        public async Task<int> EliminarTipoContactoAsync(int id)
        {
            string query = "DELETE FROM TiposContactos WHERE Id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        public async Task<int> ActualizarTipoContactoAsync(int id, TipoContactoDto tipoContacto)
        {
            string query = "UPDATE TiposContactos SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombre", tipoContacto.Nombre),
                new SqlParameter("@Descripcion", (object?)tipoContacto.Descripcion ?? DBNull.Value)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        public async Task<List<TipoContactoDto>> ObtenerTodosAsync()
        {
            // Si la tabla TiposContactos no tiene la columna Nombre, usar Descripcion como Nombre para el DTO
            string query = "SELECT Id, Descripcion AS Nombre, Descripcion FROM TiposContactos ORDER BY Descripcion";
            // Leer directamente en TipoContactoDto y devolver la lista
            var tipos = await _sqlServer.ReaderListAsync<TipoContactoDto>(query);
            return tipos;
        }

        internal async Task<int> InsertarTipoContactoAsync(Entities.TipoContacto entity)
        {
            string query = "INSERT INTO TiposContactos (Descripcion) VALUES (@Descripcion)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Descripcion", (object?)entity.Descripcion ?? DBNull.Value)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        internal async Task<int> ActualizarTipoContactoAsync(int id, Entities.TipoContacto entity)
        {
            string query = "UPDATE TiposContactos SET Descripcion = @Descripcion WHERE Id = @Id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Descripcion", (object?)entity.Descripcion ?? DBNull.Value)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        public async Task<Entities.TipoContacto> ObtenerPorIdAsync(int id)
        {
            string query = "SELECT Id, Descripcion FROM TiposContactos WHERE Id = @Id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };
            var tipo = await _sqlServer.ReaderAsync<Entities.TipoContacto>(query, parameters);
            return tipo;
        }
    }
}