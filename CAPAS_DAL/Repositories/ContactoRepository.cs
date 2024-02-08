using CAPAS_DAL.DataContext;
using CAPAS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPAS_DAL.Repositories
{
    public class ContactoRepository : IGenericRepository<Contacto>
    {
        private readonly CrudCapasContext _dbcontext;
        public ContactoRepository(CrudCapasContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(Contacto model)
        {
            _dbcontext.Contactos.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Contacto modelo = _dbcontext.Contactos.First(c => c.IdContacto == id);
            _dbcontext.Contactos.Remove(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Contacto model)
        {
            _dbcontext.Contactos.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Contacto> Obtener(int id)
        {
            return await _dbcontext.Contactos.FindAsync(id);
        }

        public async Task<IQueryable<Contacto>> ObtenerTodos()
        {
            IQueryable<Contacto> queryContactoSQL = _dbcontext.Contactos;
            return queryContactoSQL;
        }
    }
}
