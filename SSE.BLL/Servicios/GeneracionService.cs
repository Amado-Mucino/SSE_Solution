using System;
using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class GeneracionService
    {
        private readonly GeneracionRepository _generacionRepo;

        public GeneracionService()
        {
            _generacionRepo = new GeneracionRepository();
        }

        // CREATE (Lógica de Inserción)
        public string RegistrarGeneracion(Generacion nuevaGeneracion, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para gestionar generaciones.";
            }

            // 2. Validaciones Lógicas
            if (nuevaGeneracion.AñoIngreso <= 1900 || nuevaGeneracion.AñoEgreso <= 1900)
            {
                return "Debe ingresar años válidos.";
            }

            if (nuevaGeneracion.AñoEgreso <= nuevaGeneracion.AñoIngreso)
            {
                return "El año de egreso debe ser mayor al año de ingreso.";
            }

            if (nuevaGeneracion.IdCarrera <= 0)
            {
                return "Debe seleccionar la carrera a la que pertenece la generación.";
            }

            // 3. Valida duplicidad
            Generacion genExistente = _generacionRepo.ObtenerExacta(nuevaGeneracion.AñoIngreso, nuevaGeneracion.AñoEgreso, nuevaGeneracion.IdCarrera);
            if (genExistente != null)
            {
                return "Error: Esta generación ya se encuentra registrada para la carrera seleccionada.";
            }

            // 4. Guardar
            bool exito = _generacionRepo.Insertar(nuevaGeneracion);
            return exito ? "Generación registrada correctamente." : "Error al registrar la generación.";
        }

        // READ (Lógica de Lectura)
        public List<Generacion> ObtenerTodasLasGeneraciones()
        {
            // Libre lectura para llenar ComboBoxes en la pantalla de Egresados (Neta Jorge ya haz las pantallas)
            return _generacionRepo.ObtenerTodas();
        }

        // UPDATE (Lógica de Actualización)
        public string ActualizarGeneracion(Generacion generacion, string rolUsuarioActivo)
        {
            if (rolUsuarioActivo != "Administrador" && rolUsuarioActivo != "Coordinador Académico")
            {
                return "Acceso denegado: Su rol no tiene permisos para modificar generaciones.";
            }

            if (generacion.AñoEgreso <= generacion.AñoIngreso)
            {
                return "El año de egreso debe ser mayor al año de ingreso.";
            }

            // Valida choque con otro registro existente
            Generacion genExistente = _generacionRepo.ObtenerExacta(generacion.AñoIngreso, generacion.AñoEgreso, generacion.IdCarrera);
            if (genExistente != null && genExistente.IdGeneracion != generacion.IdGeneracion)
            {
                return "Error: Los datos introducidos coinciden con otra generación ya existente.";
            }

            bool exito = _generacionRepo.Actualizar(generacion);
            return exito ? "Generación actualizada correctamente." : "Error al actualizar la generación.";
        }
    }
}