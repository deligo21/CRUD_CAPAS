using CAPAS_DAL.Repositories;
using CAPAS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPAS_BLL.Service
{
    public class ContactoService : IContactoService
    {
        private readonly IGenericRepository<Contacto> _contactRepo;
        public ContactoService(IGenericRepository<Contacto> _contactRepo)
        {
            this._contactRepo = _contactRepo;
        }
        public async Task<bool> Actualizar(Contacto model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(Contacto model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<Contacto> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }

        public async Task<Contacto> ObtenerPorNombre(string nombreContacto)
        {
            IQueryable<Contacto> queryContactoSQL = await _contactRepo.ObtenerTodos();
            Contacto contacto = queryContactoSQL.Where(c => c.Nombre == nombreContacto).FirstOrDefault();
            return contacto;
        }

        public async Task<IQueryable<Contacto>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }
    }
}
