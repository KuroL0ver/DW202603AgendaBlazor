using AgendaWeb.Data.DTOS.Contactos;
using AgendaWeb.Data.DTOS.TiposContactos;
using AgendaWeb.Services;
using Microsoft.AspNetCore.Components;

namespace AgendaWeb.Components.Pages
{
    public partial class ContactoEditar
    {
        [Parameter]
        public int Id { get; set; }

        protected ContactoActualizarDto Contacto { get; set; } = new();
        protected List<TipoContactoDto> Tipos { get; set; } = new();
        protected bool Cargando { get; set; } = true;
        protected bool MensajeExito { get; set; } = false;
        protected string ErrorMessage { get; set; } = string.Empty;

        [Inject] protected ContactoServices ContactoServices { get; set; }
        [Inject] protected TipoContactoServices TipoContactoServices { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // Cargar tipos y contacto
            try
            {
                Tipos = await TipoContactoServices.ObtenerTodosAsync();
                var c = await ContactoServices.ObtenerPorIdAsync(Id);
                if (c != null)
                {
                    Contacto.Nombre = c.Nombre;
                    Contacto.Telefono = c.Telefono;
                    Contacto.Email = c.Email;
                    Contacto.TipoContactoId = c.TipoContactoId;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            Cargando = false;
        }

        protected async Task Guardar()
        {
            try
            {
                // Actualizar únicamente el tipo de contacto para no sobrescribir otros campos
                await ContactoServices.ActualizarSoloTipoContactoAsync(Id, Contacto.TipoContactoId);
                MensajeExito = true;
                // navegar de vuelta para que la lista se recargue
                Navigation.NavigateTo("/contactos");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected void Cancelar()
        {
            Navigation.NavigateTo("/contactos");
        }
    }
}
