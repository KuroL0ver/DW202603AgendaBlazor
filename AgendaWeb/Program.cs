using AgendaWeb.Components;
using AgendaWeb.Data;
using AgendaWeb.Data.Commands;
using AgendaWeb.Services;

namespace AgendaWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //el var builder es el que configura la entrada de la aplicación (es decir que cuando se ejecute nos cargue la página de inicio)
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            //Inyección de dependencias
            builder.Services.AddScoped<SQLServer>(_ => new SQLServer(connectionString));
            builder.Services.AddScoped<ContactoCommand>();
            builder.Services.AddScoped<ContactoServices>();
            // Registra los servicios para los tipos de contacto
            builder.Services.AddScoped<TipoContactoCommand>();
            builder.Services.AddScoped<TipoContactoServices>();
            //Builder sirve para que usar una inyección de dependencias dentro de otra, para este caso en usar el SQLServer dentro de los servicios

            var app = builder.Build();


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            https://aka.ms/aspnetcore-hsts. //Esto sirve para que el navegador sepa que es solo para https y mejore la seguridad 
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
