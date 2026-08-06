using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class RolService
    {
        private readonly RolRepository _rolRepo;

        public RolService()
        {
            _rolRepo = new RolRepository();
        }

        // READ (Lógica de Lectura)
        public List<Rol> ObtenerTodosLosRoles()
        {
            // Si se decide después agregar más restricciones, 
            // la validación iría aquí antes de retornar la lista, pero por ahora así se queda.
            return _rolRepo.ObtenerTodos();
        }
    }
}