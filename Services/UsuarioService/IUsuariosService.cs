using EcommerceStore.Models;

namespace EcommerceStore.Services.UsuarioService
{
    public interface IUsuariosService
    {
        //define que el service debe tener un metodo que devuelve una lista de usuarios 
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
    }
}
