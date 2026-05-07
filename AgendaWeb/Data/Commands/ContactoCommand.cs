using AgendaWeb.Data.Entities;
using Microsoft.Data.SqlClient;

namespace AgendaWeb.Data.Commands
{
    public class ContactoCommand
    {
        private readonly SQLServer _sqlServer;

        public ContactoCommand(SQLServer sqlServer)
        {
            _sqlServer = sqlServer;
        }

        public async Task<Contacto?> ObtenerPorIdAsync(int id)
        {
            string query = "SELECT c.Id, c.Nombre, c.Telefono, c.Email, c.TipoContactoId, t.Descripcion AS TipoContactoDescripcion " +
                "FROM Contactos c " +
                "LEFT JOIN TiposContactos t ON c.TipoContactoId = t.Id " +
                "WHERE c.Id = @Id";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            var list = await _sqlServer.ReaderListAsync<Contacto>(query, parameters);
            return list.FirstOrDefault();
        }

        public async Task<int> InsertarContactoAsync(Contacto contacto)
        {
            string query = "INSERT INTO Contactos (Nombre, Telefono, Email, TipoContactoId) VALUES (@Nombre, @Telefono, @Email, @TipoContactoId)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", contacto.Nombre),
                new SqlParameter("@Telefono", contacto.Telefono),
                new SqlParameter("@Email", contacto.Email),
                new SqlParameter("@TipoContactoId", (object?)contacto.TipoContactoId ?? DBNull.Value)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        public async Task<int> EliminarContactoAsync(int id)
        {
            string query = "DELETE FROM Contactos WHERE Id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        public async Task<int> ActualizarContactoAsync(int id, Contacto contacto)
        {
            string query = "UPDATE Contactos SET Nombre = @Nombre, Telefono = @Telefono, Email = @Email, TipoContactoId = @TipoContactoId WHERE Id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombre", contacto.Nombre),
                new SqlParameter("@Telefono", contacto.Telefono),
                new SqlParameter("@Email", contacto.Email),
                new SqlParameter("@TipoContactoId", (object?)contacto.TipoContactoId ?? DBNull.Value)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        // Actualiza únicamente el TipoContactoId de un contacto
        public async Task<int> ActualizarTipoContactoAsync(int id, int? tipoContactoId)
        {
            string query = "UPDATE Contactos SET TipoContactoId = @TipoContactoId WHERE Id = @Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@TipoContactoId", (object?)tipoContactoId ?? DBNull.Value)
            };
            return await _sqlServer.NonQueryAsync(query, parameters);
        }

        public async Task<List<Contacto>> ObtenerTodosAsync()
        {
            string query = "SELECT c.Id, c.Nombre, c.Telefono, c.Email, c.TipoContactoId, t.Descripcion AS TipoContactoDescripcion " +
                "FROM Contactos c " +
                "LEFT JOIN TiposContactos t ON c.TipoContactoId = t.Id " +
                //"{ WHERE Nombre LIKE '%gael%'} " +
                "ORDER BY c.Nombre";
            List<Contacto> contactos = new List<Contacto>();
            contactos = await _sqlServer.ReaderListAsync<Contacto>(query);
            return contactos;
            //c es para traer los datos del contacto
        } //t es para traer la descripcion del tipo de contacto, si es que tiene, sino se queda en null


    }
}
