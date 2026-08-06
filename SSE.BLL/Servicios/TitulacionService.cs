using System;
using System.Collections.Generic;
using SSE.DAL.Repositorios;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Servicios
{
    public class TitulacionService
    {
        private readonly TitulacionRepository _titulacionRepo;

        public TitulacionService()
        {
            _titulacionRepo = new TitulacionRepository();
        }

        // CREATE (Lógica de Registro)
        public string RegistrarTitulacion(Titulacion nuevaTitulacion, string rolUsuarioActivo)
        {
            // 1. Barrera de Seguridad
            if (rolUsuarioActivo == "Consultor (Solo Lectura)")
            {
                return "Acceso denegado: Los consultores no tienen permisos para registrar actas de titulación.";
            }

            // 2. Validaciones Básicas de Datos
            if (nuevaTitulacion.IdEgresado <= 0 || nuevaTitulacion.IdModalidad <= 0)
            {
                return "Debe seleccionar un egresado y una modalidad de titulación.";
            }

            if (string.IsNullOrWhiteSpace(nuevaTitulacion.NumActa))
            {
                return "El número de acta es obligatorio.";
            }

            if (nuevaTitulacion.FechaTitulacion > DateTime.Now)
            {
                return "La fecha de titulación no puede ser en el futuro.";
            }

            // 3. Impedide múltiples titulaciones
            Titulacion titExistente = _titulacionRepo.ObtenerPorEgresado(nuevaTitulacion.IdEgresado);
            if (titExistente != null)
            {
                return $"Error: El egresado seleccionado ya cuenta con un registro de titulación (Acta: {titExistente.NumActa}).";
            }

            // 4. Procede a guardar usando la transacción del DAL
            bool exito = _titulacionRepo.RegistrarTitulacionTransaccional(nuevaTitulacion);

            return exito ? "Titulación registrada con éxito. El estado del egresado ha sido actualizado automáticamente."
                         : "Error grave al registrar la titulación o actualizar el estado del egresado en la base de datos.";
        }

        // READ (Lógica de Lectura)
        public List<Titulacion> ObtenerTodasLasTitulaciones()
        {
            // Todos pueden ver el listado general
            return _titulacionRepo.ObtenerTodas();
        }
    }
}