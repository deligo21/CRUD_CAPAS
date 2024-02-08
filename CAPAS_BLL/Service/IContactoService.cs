using CAPAS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPAS_BLL.Service
{
    public interface IContactoService
    {
        Task<bool> Insertar(Contacto model);
        Task<bool> Actualizar(Contacto model);
        Task<bool> Eliminar(int id);
        Task<Contacto> Obtener(int id);
        Task<IQueryable<Contacto>> ObtenerTodos();

        Task<Contacto> ObtenerPorNombre(string nombreContacto);
    }
}
